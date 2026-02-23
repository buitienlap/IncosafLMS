using DevExpress.Web.Mvc;
using IncosafCMS.Core.Data;
using IncosafCMS.Core.DomainModels;
using IncosafCMS.Core.Identity;
using IncosafCMS.Core.Services;
using IncosafCMS.Web.Helpers;
using IncosafCMS.Web.Models;
using IncosafCMS.Web.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using IncosafCMS.Data; // 👈 Thêm using này nếu chưa có
using IncosafCMS.Core.DomainModels.Identity;



namespace IncosafCMS.Web.Controllers
{
	public class EquipmentsController : Controller
	{
		IService<Equipment> service;
		IUnitOfWork uow;
		IApplicationUserManager userManager;
		private readonly IService<Equipment> _equipmentService;//thêm 4.7.2025
		//private readonly IncosafCMSContext DB = new IncosafCMSContext(); // THÊM 2.8.2025
		public EquipmentsController(IService<Equipment> _service, IUnitOfWork _uow, IApplicationUserManager _userManager)
		{
			service = _service;
			uow = _uow;
			userManager = _userManager;
		}
		// GET: Equipments
		public ActionResult Index()
		{
			return View();
		}
		public static void CreateTreeViewNodesRecursive(IEnumerable<OriginalEquipment> model, MVCxTreeViewNodeCollection nodesCollection, string rootCode = "", string acctaskNode = "")
		{
			var eqType = "";
			if (!string.IsNullOrWhiteSpace(acctaskNode))
			{
				var splts = acctaskNode.Split(new char[] { '-' });
				if (splts.Length == 2)
				{
					eqType = splts[1];
				}
				else eqType = acctaskNode;
			}
			if (string.IsNullOrEmpty(rootCode))
			{
				var roots = model.Where(e => e.IsGroup == 1 && string.IsNullOrEmpty(e.ParentCode));

				foreach (var root in roots)
				{
					if (!string.IsNullOrWhiteSpace(eqType))
					{
						if(root.Code == eqType)
							CreateTreeViewNodesRecursive(model, nodesCollection, root.Code, eqType);
					}
					else
					{
						CreateTreeViewNodesRecursive(model, nodesCollection, root.Code);
					}
					
				}
			}
			else
			{
				var root = model.FirstOrDefault(e => e.Code == rootCode);
				var childs = model.Where(e => e.ParentCode == rootCode);

				if(root.IsGroup == 1)
				{
					MVCxTreeViewNode rootNode = nodesCollection.Add(root.Code + " - " + root.Name);
					foreach(var c in childs)
					{
						CreateTreeViewNodesRecursive(model, rootNode.Nodes, c.Code);
					}
				}
				else
				{
					var node = nodesCollection.Add(root.Code + " - " + root.Name);
					node.DataItem = root;
				}
			}
		}
		// GET: Equipments/Details/5
		public ActionResult Details(int id)
		{
			var model = service.GetById(id);
			if (model == null) model = new Equipment();
			GridViewHelper.SelectedEquipmentID = id;
			//ViewData["Contracts"] = User.IsInRole("KDV") ? uow.Repository<Contract>().GetAll().Where(x => x.own?.UserName == User.Identity.Name) : uow.Repository<Contract>().GetAll();
			//var acctask = model.contract?.Tasks.Where(x => x.Accreditations.Select(e => e.equiment?.Id == id).Count() > 0).FirstOrDefault();
			//var acctasks = model.contract?.Tasks;
			//ViewData["AccTasks"] = acctasks;
			//ViewData["AccTask"] = acctask ?? new AccTask();
			//ViewData["IndexOfTask"] = acctasks != null ? acctasks.IndexOf(acctask) : -1;
			return View(model);
		}

		// GET: Equipments/Create
		public ActionResult Create()
		{
			EquipmentSpecificationsList.GetEquipmentSpecifications = new List<Specifications>();
			EquipmentLoadTestsList.GetEquipmentLoadTests = new List<LoadTest>();
			EquipmentPartionList.GetEquipmentPartion = new List<EquipmentPartion>();
			EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument = new List<TechnicalDocument>();

			//ViewData["Contracts"] = User.IsInRole("KDV") ? uow.Repository<Contract>().GetAll().Where(x => x.own?.UserName == User.Identity.Name && x.Status == ApproveStatus.Waiting) : uow.Repository<Contract>().GetAll().Where(x => x.Status == ApproveStatus.Waiting);
			if (User.IsInRole("Admin"))
			{
				ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).ToList();
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user != null)
				{
					if (user.Department != null)
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).Where(x => x.DepartmentId == user.Department.Id).ToList();
					else
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(user.Id).ToList();
				}
				else
					ViewData["Contracts"] = new List<ContractViewModel>();
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Contracts"] = user != null ? ContractDataProvider.GetContractsByOwnerID(user.Id).ToList() : new List<ContractViewModel>();
			}

			return View(new Equipment());
		}

		// POST: Equipments/Create
		[HttpPost, ValidateInput(false)]
		public ActionResult Create([ModelBinder(typeof(DevExpress.Web.Mvc.DevExpressEditorsBinder))] Equipment equipment, FormCollection collection)
		{
			try
			{

				var strContractID = EditorExtension.GetValue<string>("cmbContractCreate");
				int contractID = 0;
				if (int.TryParse(strContractID, out contractID))
				{
					var contract = uow.Repository<Contract>().GetSingle(contractID);

					var strAmount = EditorExtension.GetValue<string>("speAmountCreate");
					int amount = 1;
					if (!int.TryParse(strAmount, out amount)) amount = 1;

					var strPrice = EditorExtension.GetValue<string>("spePriceCreate");
					double price = 0;
					if (!double.TryParse(strPrice, out price)) price = 0;

					var strTaskID = EditorExtension.GetValue<string>("cmbTaskOfContractCreate");
					int taskID = 0;
					if (int.TryParse(strTaskID, out taskID) && taskID > 0)
					{
						var accTask = contract.Tasks.Where(x => x.Id == taskID).FirstOrDefault();
						if (accTask != null)
						{
							for (int i = 0; i < amount; i++)
							{
								// Thêm thiết bị
								var newEquipment = new Equipment()
								{
									Id = 0,
									Name = equipment.Name,
									Code = equipment.Code,
									//No = equipment.No,
									//YearOfProduction = equipment.YearOfProduction,
									AccrPeriod = equipment.AccrPeriod, // Added by lapbt 08-jun-2023. Sử dụng để tính DateOfNext thay cho trường YearOfProduction
									//ManuFacturer = equipment.ManuFacturer,
									//Uses = equipment.Uses,
									isPrintGcn = false, //Hưng thêm 09.06.2025
									contract = contract
								};

								newEquipment.contract.customer = contract.customer;

								foreach (var partion in EquipmentPartionList.GetEquipmentPartion)
								{
									var newPartion = new EquipmentPartion()
									{
										Id = 0,
										Name = partion.Name,
										Note = partion.Note,
										Passed1 = true,
										Passed2 = partion.Passed2
									};

									newEquipment.Partions.Add(newPartion);

									uow.Repository<EquipmentPartion>().Insert(newPartion);
								}

								//loadTest - 08.06.2025 đóng do chưa dùng 
								//foreach (var loadTest in EquipmentLoadTestsList.GetEquipmentLoadTests)
								//{
								//	var newLoadTest = new LoadTest()
								//	{
								//		Id = 0,
								//		LocalTest = loadTest.LocalTest,
								//		Radius = loadTest.Radius,
								//		Passed = true,
								//		CorrespondingLoad = loadTest.CorrespondingLoad,
								//		DynamicLoad = loadTest.DynamicLoad,
								//		StaticLoad = loadTest.StaticLoad
								//	};

								//	newEquipment.LoadTests.Add(newLoadTest);

								//	uow.Repository<LoadTest>().Insert(newLoadTest);
								//}

								//24.05.2025
								foreach (var specification in EquipmentSpecificationsList.GetEquipmentSpecifications)

								{
									var newSpecification = new Specifications()
									{
										Id = 0,
										Name = specification.Name,
										Value = specification.Value,
										f_key = specification.f_key,
										f_unit = specification.f_unit
									};

									newEquipment.specifications.Add(newSpecification);

									uow.Repository<Specifications>().Insert(newSpecification);
								}

								//technicaldocument - đóng 08.06.2025 do chưa dùng
								//foreach (var technicaldocument in EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument)
								//{
								//	var newTechnicaldocument = new TechnicalDocument()
								//	{
								//		Id = 0,
								//		Name = technicaldocument.Name,
								//		Note = technicaldocument.Note,
								//		Passed = true
								//	};

								//	newEquipment.TechnicalDocuments.Add(newTechnicaldocument);

								//	uow.Repository<TechnicalDocument>().Insert(newTechnicaldocument);
								//}

								// Thêm thông tin kiểm định vào hợp đồng
								string dept = string.IsNullOrEmpty(accTask.AccTaskNote) ? "TBN" : accTask.AccTaskNote.Split('-').Length == 2 ? accTask.AccTaskNote.Split('-')[1] : "TBN";
								var prefix = DateTime.Today.ToString("yy") + ".";

								var autoAccreNumber = DataProvider.DB.Database.SqlQuery<AutoAccreNumber>("GetLastAutoAccreNumber @dept, @prefix", new SqlParameter("@dept", dept), new SqlParameter("@prefix", prefix)).FirstOrDefault();

								AutoAccreNumber nextAccreNumber = null;
								if (autoAccreNumber == null || autoAccreNumber.Prefix != prefix)
								{
									nextAccreNumber = new AutoAccreNumber()
									{
										Id = 0,
										Prefix = DateTime.Today.ToString("yy") + ".",
										Dept = dept,
										AccreNumber = 1
									};

									uow.Repository<AutoAccreNumber>().Insert(nextAccreNumber);
									uow.SaveChanges();
								}
								else
								{
									nextAccreNumber = new AutoAccreNumber()
									{
										Id = 0,
										Prefix = DateTime.Today.ToString("yy") + ".",
										Dept = dept,
										AccreNumber = autoAccreNumber.AccreNumber + 1
									};

									uow.Repository<AutoAccreNumber>().Insert(nextAccreNumber);
									uow.SaveChanges();
								}

								/* Added by lapbt
								 * 28-oct-2021. ô "Hạn kiểm định" sẽ có giá trị mặc định là "Ngày kiểm định" + YearOfProduction (năm)
								 * 08-jun-2023. Sử dụng AccrPeriod để tính DateOfNext thay cho trường YearOfProduction
								 */
								//if (!int.TryParse(equipment.YearOfProduction, out int yearOfNextAccr))
								//{
								//    yearOfNextAccr = 1;   // default if not set
								//}
								int yearOfNextAccr = Convert.ToInt32(equipment.AccrPeriod);
								yearOfNextAccr = (yearOfNextAccr >= 0 && yearOfNextAccr < 10) ? yearOfNextAccr : 1;

								var newAccreditation = new Accreditation()
								{
									Id = 0,
									equiment = newEquipment,
									NumberAcc = nextAccreNumber.AccreNumber.ToString("#00000"),
									AccrDate = contract.NgayThucHien ?? DateTime.Today,
									//DateOfNext = contract.NgayThucHien.HasValue ? contract.NgayThucHien.Value.AddYears(1) : DateTime.Today.AddYears(1),
									DateOfNext = contract.NgayThucHien.HasValue ? contract.NgayThucHien.Value.AddYears(yearOfNextAccr) : DateTime.Today.AddYears(yearOfNextAccr),
									AccrResultDate = contract.NgayThucHien ?? DateTime.Today
								};

								accTask.Accreditations.Add(newAccreditation);
								if (accTask.Amount < accTask.Accreditations.Count) accTask.Amount = accTask.Accreditations.Count;
								uow.Repository<Accreditation>().Insert(newAccreditation);

								accTask.UnitPrice = price;
								uow.Repository<AccTask>().Update(accTask);
								service.Add(newEquipment);
							}

							contract.CalValue();
							uow.Repository<Contract>().Update(contract);
							uow.SaveChanges();
						}
					}
					else
					{
						//var strUnit = EditorExtension.GetValue<string>("txtUnitCreate");
						//var strAccTaskName = EditorExtension.GetValue<string>("txtTaskNameCreate");
						//if (string.IsNullOrWhiteSpace(strAccTaskName)) return RedirectToAction("Equipment", "Home");

						//var newAccTask = new AccTask()
						//{
						//    Name = strAccTaskName,
						//    Unit = strUnit,
						//    UnitPrice = price,
						//    Amount = amount
						//};

						//for (int i = 0; i < amount; i++)
						//{
						//    // Thêm thiết bị
						//    var newEquipment = new Equipment()
						//    {
						//        Id = 0,
						//        Name = equipment.Name,
						//        Code = equipment.Code,
						//        No = equipment.No,
						//        YearOfProduction = equipment.YearOfProduction,
						//        ManuFacturer = equipment.ManuFacturer,
						//        Uses = equipment.Uses,
						//        contract = contract
						//    };

						//    newEquipment.contract.customer = contract.customer;

						//    foreach (var partion in EquipmentPartionList.GetEquipmentPartion)
						//    {
						//        var newPartion = new EquipmentPartion()
						//        {
						//            Id = 0,
						//            Name = partion.Name,
						//            Note = partion.Note,
						//            Passed1 = true,
						//            Passed2 = partion.Passed2
						//        };

						//        newEquipment.Partions.Add(newPartion);

						//        uow.Repository<EquipmentPartion>().Insert(newPartion);
						//    }

						//    foreach (var loadTest in EquipmentLoadTestsList.GetEquipmentLoadTests)
						//    {
						//        var newLoadTest = new LoadTest()
						//        {
						//            Id = 0,
						//            LocalTest = loadTest.LocalTest,
						//            Radius = loadTest.Radius,
						//            Passed = true,
						//            CorrespondingLoad = loadTest.CorrespondingLoad,
						//            DynamicLoad = loadTest.DynamicLoad,
						//            StaticLoad = loadTest.StaticLoad
						//        };

						//        newEquipment.LoadTests.Add(newLoadTest);

						//        uow.Repository<LoadTest>().Insert(newLoadTest);
						//    }

						//    foreach (var specification in EquipmentSpecificationsList.GetEquipmentSpecifications)
						//    {
						//        var newSpecification = new Specifications()
						//        {
						//            Id = 0,
						//            Name = specification.Name,
						//            Value = specification.Value
						//        };

						//        newEquipment.specifications.Add(newSpecification);

						//        uow.Repository<Specifications>().Insert(newSpecification);
						//    }

						//    foreach (var technicaldocument in EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument)
						//    {
						//        var newTechnicaldocument = new TechnicalDocument()
						//        {
						//            Id = 0,
						//            Name = technicaldocument.Name,
						//            Note = technicaldocument.Note,
						//            Passed = true
						//        };

						//        newEquipment.TechnicalDocuments.Add(newTechnicaldocument);

						//        uow.Repository<TechnicalDocument>().Insert(newTechnicaldocument);
						//    }

						//    // Thêm thông tin kiểm định vào hợp đồng

						//    var newAccreditation = new Accreditation()
						//    {
						//        Id = 0,
						//        equiment = newEquipment,
						//      NumberAcc = nextAccreNumber.AccreNumber.ToString("#00000"),
						//      AccrDate = contract.NgayThucHien ?? DateTime.Today,
						//      DateOfNext = contract.NgayThucHien.HasValue ? contract.NgayThucHien.Value.AddYears(1) : DateTime.Today.AddYears(1),
						//      AccrResultDate = contract.NgayThucHien ?? DateTime.Today,
						//    };

						//    newAccTask.Accreditations.Add(newAccreditation);
						//    uow.Repository<Accreditation>().Insert(newAccreditation);
						//    service.Add(newEquipment);
						//}

						//contract.Tasks.Add(newAccTask);
						//uow.Repository<Contract>().Update(contract);
						//uow.SaveChanges();
					}
				}

				#region Ẩn đi
				//var contract = uow.Repository<Contract>().GetSingle(equipment.contract.Id);
				//equipment.contract = contract;
				//foreach (var specification in EquipmentSpecificationsList.GetEquipmentSpecifications)
				//{
				//    var specificationCopy = new Specifications()
				//    {
				//        Id = 0,
				//        Name = specification.Name,
				//        Value = specification.Value
				//    };
				//    equipment.specifications.Add(specificationCopy);
				//    uow.Repository<Specifications>().Insert(specificationCopy);
				//    uow.SaveChanges();
				//}

				//foreach (var partion in EquipmentPartionList.GetEquipmentPartion)
				//{
				//    var partionCopy = new EquipmentPartion()
				//    {
				//        Id = 0,
				//        Name = partion.Name,
				//        Passed1 = partion.Passed1,
				//        Passed2 = partion.Passed2,
				//        Note = partion.Note
				//    };
				//    equipment.Partions.Add(partionCopy);
				//    uow.Repository<EquipmentPartion>().Insert(partionCopy);
				//    uow.SaveChanges();
				//}
				//service.Add(equipment); 
				#endregion
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return RedirectToAction("Equipment", "Home");
		}

		// GET: Equipments/Edit/5
		public ActionResult Edit(int id)
		{

			// 18-apr-2025. Nút Chỉnh sửa: Chỉ admin truy cập được (để thay đổi Mã TB), các role khác tạm thời chưa cho truy cập vì cần sửa cửa sổ đó một chút
			if (!User.IsInRole("Admin"))
			{
				ViewData["EditError"] = "Không có quyền chỉnh sửa thiết bị.";
				return PartialView("ErrorMessage");
			}
			// Các code xử lý sửa phía sau vẫn giữ nguyên

			var model = service.GetById(id);
			if (model == null) model = new Equipment();
			EquipmentSpecificationsList.GetEquipmentSpecifications = model.specifications ?? new List<Specifications>();
			EquipmentPartionList.GetEquipmentPartion = model.Partions ?? new List<EquipmentPartion>();
			EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument = model.TechnicalDocuments ?? new List<TechnicalDocument>();
			EquipmentLoadTestsList.GetEquipmentLoadTests = model.LoadTests ?? new List<LoadTest>();

			//ViewData["Contracts"] = User.IsInRole("KDV") ? uow.Repository<Contract>().GetAll().Where(x => x.own?.UserName == User.Identity.Name) : uow.Repository<Contract>().GetAll();

			if (User.IsInRole("Admin"))
			{
				ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).ToList();
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user != null)
				{
					if (user.Department != null)
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).Where(x => x.DepartmentId == user.Department.Id).ToList();
					else
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(user.Id).ToList();
				}
				else
					ViewData["Contracts"] = new List<ContractViewModel>();
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Contracts"] = user != null ? ContractDataProvider.GetContractsByOwnerID(user.Id).ToList() : new List<ContractViewModel>();
			}

			//var acctask = model.contract.Tasks.Where(x => x.Accreditations.Select(e => e.equiment?.Id == id).Count() > 0).FirstOrDefault();
			//var acctasks = new List<AccTask>();
			//acctasks.Add(new AccTask() { Id = -1, Name = "[Tạo mới công việc]" });
			//foreach (var acct in model.contract.Tasks)
			//{
			//    acctasks.Add(acct);
			//}
			//ViewData["AccTasks"] = acctasks;
			//ViewData["AccTask"] = acctask;
			//ViewData["IndexOfTask"] = acctasks.IndexOf(acctask);

			return View(model);
		}

		// POST: Equipments/Edit/5
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var equipment = service.GetById(id);
					if (equipment != null)
					{
						#region Ẩn đi
						//var strContractID = EditorExtension.GetValue<string>("cmbContractEdit");
						//int contractID = 0;
						//if (int.TryParse(strContractID, out contractID) && contractID > 0)
						//{
						//    var contract = uow.Repository<Contract>().GetSingle(contractID);

						//    var strAmount = EditorExtension.GetValue<string>("speAmountEdit");
						//    int amount = 1;
						//    if (!int.TryParse(strAmount, out amount)) amount = 1;

						//    var strTaskName = EditorExtension.GetValue<string>("txtTaskNameEdit");
						//    var strTaskUnit = EditorExtension.GetValue<string>("txtUnitEdit");

						//    var strPrice = EditorExtension.GetValue<string>("spePriceEdit");
						//    double price = 0;
						//    if (!double.TryParse(strPrice, out price)) price = 0;

						//    var strTaskID = EditorExtension.GetValue<string>("cmbTaskOfContractEdit");
						//    int taskID = 0;
						//    if (int.TryParse(strTaskID, out taskID) && taskID > 0)
						//    {
						//        var Accreditations = uow.Repository<Accreditation>().GetAll();
						//        var accreditationOld = Accreditations.Where(x => x.equiment?.Id == equipment.Id).FirstOrDefault();
						//        var taskOld = equipment.contract.Tasks.Where(x => x.Accreditations.Select(e => e.equiment?.Id == equipment.Id).Count() > 0).FirstOrDefault();
						//        if (equipment?.contract.Id != contractID)
						//        {
						//            taskOld?.Accreditations.Remove(accreditationOld);

						//            var taskNew = contract.Tasks.Where(x => x.Id == taskID).FirstOrDefault();
						//            taskNew.Accreditations.Add(accreditationOld);
						//            taskNew.Name = strTaskName;
						//            taskNew.Unit = strTaskUnit;
						//            taskNew.UnitPrice = price;

						//            equipment.contract.customer = equipment.contract.customer;
						//            uow.Repository<Contract>().Update(equipment.contract);
						//            equipment.contract = contract;
						//            equipment.contract.customer = contract.customer;
						//            uow.Repository<Equipment>().Update(equipment);
						//            uow.Repository<Contract>().Update(contract);
						//            uow.SaveChanges();
						//        }
						//        else
						//        {
						//            if (taskOld?.Id != taskID)
						//            {
						//                taskOld.Accreditations.Remove(accreditationOld);

						//                var taskNew = equipment.contract.Tasks.Where(x => x.Id == taskID).FirstOrDefault();
						//                taskNew.Accreditations.Add(accreditationOld);
						//                taskNew.Name = strTaskName;
						//                taskNew.Unit = strTaskUnit;
						//                taskNew.UnitPrice = price;

						//                uow.Repository<Contract>().Update(equipment.contract);
						//                uow.Repository<Contract>().Update(contract);
						//            }
						//        }
						//    }
						//    else
						//    {
						//        var Accreditations = uow.Repository<Accreditation>().GetAll();
						//        var accreditationOld = Accreditations.Where(x => x.equiment?.Id == equipment.Id).FirstOrDefault();
						//        var taskOld = equipment?.contract.Tasks.Where(x => x.Accreditations.Select(e => e.equiment?.Id == equipment.Id).Count() > 0).FirstOrDefault();

						//        if (equipment?.contract.Id != contractID)
						//        {
						//            taskOld?.Accreditations.Remove(accreditationOld);

						//            var taskNew = new AccTask()
						//            {
						//                Id = 0,
						//                Name = strTaskName,
						//                Unit = strTaskUnit,
						//                UnitPrice = price
						//            };
						//            taskNew.Accreditations.Add(accreditationOld);
						//            contract.Tasks.Add(taskNew);

						//            equipment.contract.customer = equipment.contract.customer;
						//            uow.Repository<Contract>().Update(equipment.contract);
						//            equipment.contract = contract;
						//            equipment.contract.customer = contract.customer;
						//            uow.Repository<Equipment>().Update(equipment);
						//            uow.SaveChanges();
						//        }
						//        else
						//        {
						//            if (taskOld?.Id != taskID)
						//            {
						//                taskOld?.Accreditations.Remove(accreditationOld);

						//                var taskNew = new AccTask()
						//                {
						//                    Id = 0,
						//                    Name = strTaskName,
						//                    Unit = strTaskUnit,
						//                    UnitPrice = price
						//                };
						//                taskNew.Accreditations.Add(accreditationOld);
						//                equipment.contract.Tasks.Add(taskNew);

						//                uow.Repository<Contract>().Update(equipment.contract);
						//                uow.SaveChanges();
						//            }
						//        }
						//    }


						//    equipment.Code = EditorExtension.GetValue<string>("Code");
						//    equipment.Name = EditorExtension.GetValue<string>("Name");
						//    equipment.No = EditorExtension.GetValue<string>("No");
						//    equipment.YearOfProduction = EditorExtension.GetValue<string>("YearOfProduction");
						//    equipment.ManuFacturer = EditorExtension.GetValue<string>("ManuFacturer");
						//    equipment.Uses = EditorExtension.GetValue<string>("Uses");

						//    // Xử lý Partions
						//    for (int i = 0; i < equipment.Partions.Count; i++)
						//    {
						//        var originalPartion = equipment.Partions[i];
						//        var modifiedPartion = EquipmentPartionList.GetEquipmentPartion.Find(x => x.Id == originalPartion.Id);
						//        if (modifiedPartion != null)
						//        {
						//            originalPartion.Name = modifiedPartion.Name;
						//            originalPartion.Passed1 = modifiedPartion.Passed1;
						//            originalPartion.Passed2 = modifiedPartion.Passed2;
						//            originalPartion.Note = modifiedPartion.Note;

						//            uow.Repository<EquipmentPartion>().Update(originalPartion);
						//            //uow.SaveChanges();
						//        }
						//        else
						//        {
						//            equipment.Partions.Remove(originalPartion);

						//            uow.Repository<EquipmentPartion>().Delete(originalPartion);
						//            //uow.SaveChanges();
						//            i--;
						//        }
						//    }

						//    foreach (var partionNew in EquipmentPartionList.GetEquipmentPartion)
						//    {
						//        var oriPartion = equipment.Partions.Find(x => x.Id == partionNew.Id);
						//        if (oriPartion != null) continue;
						//        var partionNewCopy = new EquipmentPartion()
						//        {
						//            Id = 0,
						//            Name = partionNew.Name,
						//            Passed1 = partionNew.Passed1,
						//            Passed2 = partionNew.Passed2,
						//            Note = partionNew.Note
						//        };
						//        equipment.Partions.Add(partionNewCopy);
						//        uow.Repository<EquipmentPartion>().Insert(partionNewCopy);
						//        //uow.SaveChanges();
						//    }

						//    // Xử lý LoadTest
						//    for (int i = 0; i < equipment.LoadTests.Count; i++)
						//    {
						//        var originalLoadTest = equipment.LoadTests[i];
						//        var modifiedLoadTest = EquipmentLoadTestsList.GetEquipmentLoadTests.Find(x => x.Id == originalLoadTest.Id);
						//        if (modifiedLoadTest != null)
						//        {
						//            originalLoadTest.LocalTest = modifiedLoadTest.LocalTest;
						//            originalLoadTest.Radius = modifiedLoadTest.Radius;
						//            originalLoadTest.Passed = modifiedLoadTest.Passed;
						//            originalLoadTest.CorrespondingLoad = modifiedLoadTest.CorrespondingLoad;
						//            originalLoadTest.DynamicLoad = modifiedLoadTest.DynamicLoad;
						//            originalLoadTest.StaticLoad = modifiedLoadTest.StaticLoad;

						//            uow.Repository<LoadTest>().Update(originalLoadTest);
						//            //uow.SaveChanges();
						//        }
						//        else
						//        {
						//            equipment.LoadTests.Remove(originalLoadTest);

						//            uow.Repository<LoadTest>().Delete(originalLoadTest);
						//            //uow.SaveChanges();
						//            i--;
						//        }
						//    }

						//    foreach (var loadTestNew in EquipmentLoadTestsList.GetEquipmentLoadTests)
						//    {
						//        var oriLoadTest = equipment.LoadTests.Find(x => x.Id == loadTestNew.Id);
						//        if (oriLoadTest != null) continue;
						//        var loadTestNewCopy = new LoadTest()
						//        {
						//            Id = 0,
						//            LocalTest = loadTestNew.LocalTest,
						//            Radius = loadTestNew.Radius,
						//            Passed = loadTestNew.Passed,
						//            CorrespondingLoad = loadTestNew.CorrespondingLoad,
						//            DynamicLoad = loadTestNew.DynamicLoad,
						//            StaticLoad = loadTestNew.StaticLoad
						//        };
						//        equipment.LoadTests.Add(loadTestNewCopy);
						//        uow.Repository<LoadTest>().Insert(loadTestNewCopy);
						//        //uow.SaveChanges();
						//    }

						//    // Xử lý Specifications
						//    for (int i = 0; i < equipment.specifications.Count; i++)
						//    {
						//        var originalSpecifications = equipment.specifications[i];
						//        var modifiedSpecifications = EquipmentSpecificationsList.GetEquipmentSpecifications.Find(x => x.Id == originalSpecifications.Id);
						//        if (modifiedSpecifications != null)
						//        {
						//            originalSpecifications.Name = modifiedSpecifications.Name;
						//            originalSpecifications.Value = modifiedSpecifications.Value;

						//            uow.Repository<Specifications>().Update(originalSpecifications);
						//            //uow.SaveChanges();
						//        }
						//        else
						//        {
						//            equipment.specifications.Remove(originalSpecifications);

						//            uow.Repository<Specifications>().Delete(originalSpecifications);
						//            //uow.SaveChanges();
						//            i--;
						//        }
						//    }

						//    foreach (var specificationsNew in EquipmentSpecificationsList.GetEquipmentSpecifications)
						//    {
						//        var oriSpecifications = equipment.specifications.Find(x => x.Id == specificationsNew.Id);
						//        if (oriSpecifications != null) continue;
						//        var specificationsNewCopy = new Specifications()
						//        {
						//            Id = 0,
						//            Name = specificationsNew.Name,
						//            Value = specificationsNew.Value
						//        };
						//        equipment.specifications.Add(specificationsNewCopy);
						//        uow.Repository<Specifications>().Insert(specificationsNewCopy);
						//        //uow.SaveChanges();
						//    }

						//    // Xử lý TechnicalDocument
						//    for (int i = 0; i < equipment.TechnicalDocuments.Count; i++)
						//    {
						//        var originalTechnicalDocument = equipment.TechnicalDocuments[i];
						//        var modifiedTechnicalDocument = EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument.Find(x => x.Id == originalTechnicalDocument.Id);
						//        if (modifiedTechnicalDocument != null)
						//        {
						//            originalTechnicalDocument.Name = modifiedTechnicalDocument.Name;
						//            originalTechnicalDocument.Note = modifiedTechnicalDocument.Note;
						//            originalTechnicalDocument.Passed = modifiedTechnicalDocument.Passed;

						//            uow.Repository<TechnicalDocument>().Update(originalTechnicalDocument);
						//            //uow.SaveChanges();
						//        }
						//        else
						//        {
						//            equipment.TechnicalDocuments.Remove(originalTechnicalDocument);

						//            uow.Repository<TechnicalDocument>().Delete(originalTechnicalDocument);
						//            //uow.SaveChanges();
						//            i--;
						//        }
						//    }

						//    foreach (var technicalDocumentNew in EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument)
						//    {
						//        var oriTechnicalDocuments = equipment.TechnicalDocuments.Find(x => x.Id == technicalDocumentNew.Id);
						//        if (oriTechnicalDocuments != null) continue;
						//        var technicalDocumentNewCopy = new TechnicalDocument()
						//        {
						//            Id = 0,
						//            Name = technicalDocumentNew.Name,
						//            Note = technicalDocumentNew.Note,
						//            Passed = true
						//        };
						//        equipment.TechnicalDocuments.Add(technicalDocumentNewCopy);
						//        uow.Repository<TechnicalDocument>().Insert(technicalDocumentNewCopy);
						//        //uow.SaveChanges();
						//    }

						//    uow.SaveChanges();
						//    service.Update(equipment);
						//}

						#endregion
						equipment.Code = EditorExtension.GetValue<string>("Code");
						equipment.Name = EditorExtension.GetValue<string>("Name");
						equipment.No = EditorExtension.GetValue<string>("No");
						equipment.YearOfProduction = EditorExtension.GetValue<string>("YearOfProduction");
						equipment.ManuFacturer = EditorExtension.GetValue<string>("ManuFacturer");
						equipment.Uses = EditorExtension.GetValue<string>("Uses");

						// Xử lý Partions
						for (int i = 0; i < equipment.Partions.Count; i++)
						{
							var originalPartion = equipment.Partions[i];
							var modifiedPartion = EquipmentPartionList.GetEquipmentPartion.Find(x => x.Id == originalPartion.Id);
							if (modifiedPartion != null)
							{
								originalPartion.Name = modifiedPartion.Name;
								originalPartion.Passed1 = modifiedPartion.Passed1;
								originalPartion.Passed2 = modifiedPartion.Passed2;
								originalPartion.Note = modifiedPartion.Note;

								uow.Repository<EquipmentPartion>().Update(originalPartion);
								//uow.SaveChanges();
							}
							else
							{
								equipment.Partions.Remove(originalPartion);

								uow.Repository<EquipmentPartion>().Delete(originalPartion);
								//uow.SaveChanges();
								i--;
							}
						}

						foreach (var partionNew in EquipmentPartionList.GetEquipmentPartion)
						{
							var oriPartion = equipment.Partions.Find(x => x.Id == partionNew.Id);
							if (oriPartion != null) continue;
							var partionNewCopy = new EquipmentPartion()
							{
								Id = 0,
								Name = partionNew.Name,
								Passed1 = partionNew.Passed1,
								Passed2 = partionNew.Passed2,
								Note = partionNew.Note
							};
							equipment.Partions.Add(partionNewCopy);
							uow.Repository<EquipmentPartion>().Insert(partionNewCopy);
							//uow.SaveChanges();
						}

						// Xử lý LoadTest - đóng 08.06.2025 do chưa dùng
						//for (int i = 0; i < equipment.LoadTests.Count; i++)
						//{
						//	var originalLoadTest = equipment.LoadTests[i];
						//	var modifiedLoadTest = EquipmentLoadTestsList.GetEquipmentLoadTests.Find(x => x.Id == originalLoadTest.Id);
						//	if (modifiedLoadTest != null)
						//	{
						//		originalLoadTest.LocalTest = modifiedLoadTest.LocalTest;
						//		originalLoadTest.Radius = modifiedLoadTest.Radius;
						//		originalLoadTest.Passed = modifiedLoadTest.Passed;
						//		originalLoadTest.CorrespondingLoad = modifiedLoadTest.CorrespondingLoad;
						//		originalLoadTest.DynamicLoad = modifiedLoadTest.DynamicLoad;
						//		originalLoadTest.StaticLoad = modifiedLoadTest.StaticLoad;

						//		uow.Repository<LoadTest>().Update(originalLoadTest);
						//		//uow.SaveChanges();
						//	}
						//	else
						//	{
						//		equipment.LoadTests.Remove(originalLoadTest);

						//		uow.Repository<LoadTest>().Delete(originalLoadTest);
						//		//uow.SaveChanges();
						//		i--;
						//	}
						//}

						//foreach (var loadTestNew in EquipmentLoadTestsList.GetEquipmentLoadTests)
						//{
						//	var oriLoadTest = equipment.LoadTests.Find(x => x.Id == loadTestNew.Id);
						//	if (oriLoadTest != null) continue;
						//	var loadTestNewCopy = new LoadTest()
						//	{
						//		Id = 0,
						//		LocalTest = loadTestNew.LocalTest,
						//		Radius = loadTestNew.Radius,
						//		Passed = loadTestNew.Passed,
						//		CorrespondingLoad = loadTestNew.CorrespondingLoad,
						//		DynamicLoad = loadTestNew.DynamicLoad,
						//		StaticLoad = loadTestNew.StaticLoad
						//	};
						//	equipment.LoadTests.Add(loadTestNewCopy);
						//	uow.Repository<LoadTest>().Insert(loadTestNewCopy);
						//	//uow.SaveChanges();
						//}
						//End xly LoadTest

						//24.05.2025 Xử lý update Specifications
						for (int i = 0; i < equipment.specifications.Count; i++)
						{
							var originalSpecifications = equipment.specifications[i];
							var modifiedSpecifications = EquipmentSpecificationsList.GetEquipmentSpecifications.Find(x => x.Id == originalSpecifications.Id);
							if (modifiedSpecifications != null)
							{
								originalSpecifications.Name = modifiedSpecifications.Name;
								originalSpecifications.Value = modifiedSpecifications.Value;
								originalSpecifications.f_key = modifiedSpecifications.f_key;
								originalSpecifications.f_unit = modifiedSpecifications.f_unit;

								uow.Repository<Specifications>().Update(originalSpecifications);
								//uow.SaveChanges();
							}
							else
							{
								equipment.specifications.Remove(originalSpecifications);

								uow.Repository<Specifications>().Delete(originalSpecifications);
								//uow.SaveChanges();
								i--;
							}
						}

						foreach (var specificationsNew in EquipmentSpecificationsList.GetEquipmentSpecifications)
						{
							var oriSpecifications = equipment.specifications.Find(x => x.Id == specificationsNew.Id);
							if (oriSpecifications != null) continue;
							var specificationsNewCopy = new Specifications()
							{
								Id = 0,
								Name = specificationsNew.Name,
								Value = specificationsNew.Value,
								f_key = specificationsNew.f_key,
								f_unit = specificationsNew.f_unit
							};
							equipment.specifications.Add(specificationsNewCopy);
							uow.Repository<Specifications>().Insert(specificationsNewCopy);
							//uow.SaveChanges();
						}

						// Xử lý TechnicalDocument - 08.06.2025 đóng do chưa dùng 
						//for (int i = 0; i < equipment.TechnicalDocuments.Count; i++)
						//{
						//	var originalTechnicalDocument = equipment.TechnicalDocuments[i];
						//	var modifiedTechnicalDocument = EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument.Find(x => x.Id == originalTechnicalDocument.Id);
						//	if (modifiedTechnicalDocument != null)
						//	{
						//		originalTechnicalDocument.Name = modifiedTechnicalDocument.Name;
						//		originalTechnicalDocument.Note = modifiedTechnicalDocument.Note;
						//		originalTechnicalDocument.Passed = modifiedTechnicalDocument.Passed;

						//		uow.Repository<TechnicalDocument>().Update(originalTechnicalDocument);
						//		//uow.SaveChanges();
						//	}
						//	else
						//	{
						//		equipment.TechnicalDocuments.Remove(originalTechnicalDocument);

						//		uow.Repository<TechnicalDocument>().Delete(originalTechnicalDocument);
						//		//uow.SaveChanges();
						//		i--;
						//	}
						//}

						//technicalDocument - 08.06.2025 đóng do chưa dùng 
						//foreach (var technicalDocumentNew in EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument)
						//{
						//	var oriTechnicalDocuments = equipment.TechnicalDocuments.Find(x => x.Id == technicalDocumentNew.Id);
						//	if (oriTechnicalDocuments != null) continue;
						//	var technicalDocumentNewCopy = new TechnicalDocument()
						//	{
						//		Id = 0,
						//		Name = technicalDocumentNew.Name,
						//		Note = technicalDocumentNew.Note,
						//		Passed = true
						//	};
						//	equipment.TechnicalDocuments.Add(technicalDocumentNewCopy);
						//	uow.Repository<TechnicalDocument>().Insert(technicalDocumentNewCopy);
						//	//uow.SaveChanges();
						//}

						uow.SaveChanges();
						service.Update(equipment);
					}
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return RedirectToAction("Equipment", "Home");
		}

		
		// GET: Equipments/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Equipments/Delete/5
		[HttpPost]
		public ActionResult Delete(int id, FormCollection collection)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var equipment = service.FindBy(x => x.Id == id).FirstOrDefault();
					if (equipment != null)
					{
						// 18-apr-2025. Nút xóa: chỉ cho xóa TB khi cột Mã HĐ trống (chưa cấp số HĐ); riêng admin vẫn xóa được.
						if (!User.IsInRole("Admin") && !string.IsNullOrEmpty(equipment.contract.MaHD))
						{
							//truyền luôn nội dung lỗi về, để show message                            
							return Json("Hợp đồng đã cấp số, không thể xóa thiết bị này.", JsonRequestBehavior.AllowGet);
						}

						equipment.contract = null;
						var partions = equipment.Partions;
						for (int i = 0; i < partions.Count; i++)
						{
							var partion = partions[i];
							equipment.Partions.Remove(partion);
							uow.Repository<EquipmentPartion>().Delete(partion);
							i--;
						}

						//var loadTests = equipment.LoadTests;
						//for (int i = 0; i < loadTests.Count; i++)
						//{
						//	var loadTest = loadTests[i];
						//	equipment.LoadTests.Remove(loadTest);
						//	uow.Repository<LoadTest>().Delete(loadTest);
						//	i--;
						//}

						var specifications = equipment.specifications;
						for (int i = 0; i < specifications.Count; i++)
						{
							var specification = specifications[i];
							equipment.specifications.Remove(specification);
							uow.Repository<Specifications>().Delete(specification);
							i--;
						}

						//var technicaldocuments = equipment.TechnicalDocuments;
						//for (int i = 0; i < technicaldocuments.Count; i++)
						//{
						//	var technicaldocument = technicaldocuments[i];
						//	equipment.TechnicalDocuments.Remove(technicaldocument);
						//	uow.Repository<TechnicalDocument>().Delete(technicaldocument);
						//	i--;
						//}

						var accreditation = uow.Repository<Accreditation>().FindBy(x => x.equiment != null && x.equiment.Id == equipment.Id).FirstOrDefault();
						if (accreditation != null)
						{
							//accreditation.EmployedProcedure = null;
							//accreditation.EmployedStandard = null;
							//accreditation.equiment = null;
							uow.Repository<Accreditation>().Delete(accreditation);
						}

						uow.SaveChanges();
						service.Delete(equipment);
						return Json("success", JsonRequestBehavior.AllowGet);
					}
					return Json("error", JsonRequestBehavior.AllowGet);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					return Json("error", JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				ViewData["EditError"] = "Please, correct all errors.";
				return Json("error", JsonRequestBehavior.AllowGet);
			}
			//return RedirectToAction("Equipment", "Home");
		}

		#region Details View

		/*
		 * Mô tả hoạt động và hướng xử lý cải tiến page Danh sách thiết bị
		 * 14-apr-2025 by Lapbt
		 * 0. Liên quan
		 * - View Equipment.cshtml 
		 *      - Là page chính, sau lớp layout. Chứa nút menu, filter, nhúng Partial _EquimentViewPartical.cshtml
		 *      - Chứa script để xử lý các sự kiện (filter, row change) của grid
		 * - Partial _EquimentViewPartical.cshtml
		 *      - Là vùng hiển thị grid
		 *      - Đặt setting cho grid bao gồm thuộc tính và phương thức (gọi về controller, script)
		 * - EquipmentController.cs
		 *      - Thực hiện xly dữ liệu
		 *      
		 * 1. Hoạt động
		 * - khi load ban đầu sẽ được khởi tạo ds tbi bởi hàm ActionResult EquipmentViewPartial()
		 * - khi filter script sẽ gọi về ActionResult ChangeEquipmentGridFilterModePartial(int filtermode). Bản chất sau đó lại gọi về EquipmentViewPartial()
		 * - Vấn đề
		 *      - Chậm: do lúc nào cg load toàn bộ DL, mà k0 có giới hạn
		 *      - Khi load page thấy hiện tượng call 2 lần. Có thể lần đầu gọi để show grid, sau đó lần 2 đc gọi khi thiết lập setting cho filter, nên bị "nhiễm sự kiện" filter
		 *      
		 * 2. Xử lý
		 * - Theo yc bỏ hết phần filter -> nên sẽ bỏ triệt để chứ k0 fai chỉ ẩn ở giao diện, để k0 bị gọi lặp
		 * - Xử lý lại cách tương tác vào DL bằng 
		 *      - sửa proc thêm vào 2 biến ngày `@accrfromdate as nvarchar(15), @accrtodate as nvarchar(15)`. Thêm đk `and (accre.AccrDate between @accrfromdate and @accrtodate)`
		 *      - sửa lại code ở provider để thêm luôn 1 method mới AllEquipmentsFilter có thêm 2 biến ngày
		 *      - sửa code gọi ở đây, dùng method mới
		 */

		[ValidateInput(false)]
		public ActionResult EquipmentViewPartial()
		{
			// 14-apr-2025. Mặc định kích hoạt Adv filter. Và giới hạn SL theo ngày KĐ từ & đến
			DateTime fromdate;
			DateTime todate;
			if (GridViewHelper.IsAdvancedEquipmentFilterFromDate)
			{
				if (GridViewHelper.AdvancedEquipmentFilterFromDate != null && GridViewHelper.AdvancedEquipmentFilterToDate != null)
				{
					fromdate = GridViewHelper.AdvancedEquipmentFilterFromDate;
					todate = GridViewHelper.AdvancedEquipmentFilterToDate.AddDays(1);
				}
				else
				{
					fromdate = DateTime.Today.AddYears(-3); //Lấy DL 3 năm gần nhất
					todate = DateTime.Today.AddDays(1);
				}
			}
			else
			{
				fromdate = DateTime.Today.AddYears(-5);     // full data thì cũng lấy 5 năm gần nhất 
				todate = DateTime.Today.AddYears(1);
			}

			// var equipments = EquipmentDataProvider.AllEquipments.OrderByDescending(e => e.ContractSignDate);//.ToList();
			var equipments = EquipmentDataProvider.AllEquipmentsFilter(fromdate, todate).OrderByDescending(e => e.ContractSignDate);    // use new method
			var model = new List<EquipmentViewModel>();

			model = equipments.Where(e => !string.IsNullOrEmpty(e.AccreResultNumber)).ToList();

			if (GridViewHelper.IsAdvancedEquipmentFilterFromDate)
			{
				if (GridViewHelper.AdvancedEquipmentFilterEmployeeID > 0)
					model = model.Where(x => x.OwnerID == GridViewHelper.AdvancedEquipmentFilterEmployeeID).ToList();
				if (GridViewHelper.AdvancedEquipmentFilterDepartmentID > 0)
					model = model.Where(x => x.DepartmentID == GridViewHelper.AdvancedEquipmentFilterDepartmentID).ToList();
			}

			switch (GridViewHelper.EquipmentGridFilterIndex)
			{
				case 0: // 14-apr-2025. Bỏ đi filter option, nên chỉ duy nhất xảy ra t.hợp index = 0
					if (User.IsInRole("Admin")) {
						// model = equipments.ToList();
					}
					else if (User.IsInRole("DeptDirector"))
					{
						var user = userManager.FindByName(User.Identity.Name);
						if (user != null)
							model = model.Where(x => x.DepartmentID == user.Department.Id).ToList();
						else
							model = model.Where(x => x.OwnerUserName == User.Identity.Name).ToList();
					}
					else
						model = model.Where(x => x.OwnerUserName == User.Identity.Name).ToList();
					break;
				case 1:
					if (User.IsInRole("Admin"))
						model = equipments.Where(x => x.NextAccreDate != null && x.NextAccreDate.Value.AddDays(-30) == DateTime.Today).ToList();
					else if (User.IsInRole("DeptDirector"))
					{
						var user = userManager.FindByName(User.Identity.Name);
						if (user != null)
							model = equipments.Where(x =>
									x.DepartmentID == user.Department.Id && x.NextAccreDate != null &&
									x.NextAccreDate.Value.AddDays(-30) ==
									DateTime.Today)
								.ToList();
						else
							model = equipments.Where(x =>
									x.OwnerUserName == User.Identity.Name && x.NextAccreDate != null &&
									x.NextAccreDate.Value.AddDays(-30) ==
									DateTime.Today)
								.ToList();
					}
					else
					{
						// Edited by Lapbt 09-Oct-2020. Err: xem thiet bi sap het han, loi su dung ham
						// model = equipments.AsQueryable().Where(x => x.OwnerUserName == User.Identity.Name &&
						//x.NextAccreDate != null && System.Data.Entity.DbFunctions.AddDays((DateTime)x.NextAccreDate, -30) ==
						//DateTime.Today).ToList();

						model = equipments.AsQueryable().Where(x => x.OwnerUserName == User.Identity.Name &&
							x.NextAccreDate != null && x.NextAccreDate.Value.AddDays(-30) ==
									DateTime.Today).ToList();
					}
					break;
				case 2:
					if (User.IsInRole("Admin"))
						model = equipments.Where(x => x.NextAccreDate != null && x.NextAccreDate < DateTime.Today).ToList();
					else if (User.IsInRole("DeptDirector"))
					{
						var user = userManager.FindByName(User.Identity.Name);
						if (user != null)
							model = equipments.Where(x => x.DepartmentID == user.Department.Id && x.NextAccreDate != null && x.NextAccreDate < DateTime.Today).ToList();
						else
							model = equipments.Where(x => x.OwnerUserName == User.Identity.Name && x.NextAccreDate != null && x.NextAccreDate < DateTime.Today).ToList();
					}
					else
						model = equipments.Where(x => x.OwnerUserName == User.Identity.Name && x.NextAccreDate != null && x.NextAccreDate < DateTime.Today).ToList();
					break;
			}

			
			return PartialView("_EquipmentViewPartial", model);
		}
		public ActionResult ChangeEquipmentGridFilterModePartial(int filtermode)
		{
			GridViewHelper.EquipmentGridFilterIndex = filtermode;
			return EquipmentViewPartial();
		}
		[ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentPartial()
		{
			if (GridViewHelper.SelectedEquipmentID < 0)
			{
				var model = new Equipment();
				return PartialView("_LoadTestsOfEquipmentPartial", model);
			}
			else
			{
				//var equipments = IncosafCMS.Web.Providers.EquipmentDataProvider.Equipments.ToList();
				//var model = equipments.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentID);
				//if (model?.LoadTests == null) model = new Equipment();

				var model = EquipmentDataProvider.GetLoadTests(GridViewHelper.SelectedEquipmentID);
				return PartialView("_LoadTestsOfEquipmentPartial", model);
			}
		}
		public ActionResult CustomCallBackLoadTestsOfEquipmentAction(int selectedequipment)
		{
			if (string.IsNullOrEmpty(selectedequipment.ToString()) || selectedequipment < 0) GridViewHelper.SelectedEquipmentID = -1;
			else GridViewHelper.SelectedEquipmentID = selectedequipment;
			return LoadTestsOfEquipmentPartial();
		}
		[ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentPartial()
		{
			if (GridViewHelper.SelectedEquipmentID < 0)
			{
				var model = new Equipment();
				return PartialView("_SpecificationsOfEquipmentPartial", model);
			}
			else
			{
				//var equipments = IncosafCMS.Web.Providers.EquipmentDataProvider.Equipments.ToList();
				//var model = equipments.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentID);
				//if (model?.specifications == null) model = new Equipment();
				var model = EquipmentDataProvider.GetSpecifications(GridViewHelper.SelectedEquipmentID);
				return PartialView("_SpecificationsOfEquipmentPartial", model);
			}
		}
		public ActionResult CustomCallBackSpecificationsOfEquipmentAction(int selectedequipment)
		{
			if (string.IsNullOrEmpty(selectedequipment.ToString()) || selectedequipment < 0) GridViewHelper.SelectedEquipmentID = -1;
			else GridViewHelper.SelectedEquipmentID = selectedequipment;
			return SpecificationsOfEquipmentPartial();
		}
		[ValidateInput(false)]
		public ActionResult PartionsOfEquipmentPartial()
		{
			if (GridViewHelper.SelectedEquipmentID < 0)
			{
				var model = new Equipment();
				return PartialView("_PartionsOfEquipmentPartial", model);
			}
			else
			{
				//var equipments = IncosafCMS.Web.Providers.EquipmentDataProvider.Equipments.ToList();
				//var model = equipments.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentID);
				//if (model?.Partions == null) model = new Equipment();
				var model = EquipmentDataProvider.GetPartions(GridViewHelper.SelectedEquipmentID);
				return PartialView("_PartionsOfEquipmentPartial", model);
			}
		}
		public ActionResult CustomCallBackPartionsOfEquipmentAction(int selectedequipment)
		{
			if (string.IsNullOrEmpty(selectedequipment.ToString()) || selectedequipment < 0) GridViewHelper.SelectedEquipmentID = -1;
			else GridViewHelper.SelectedEquipmentID = selectedequipment;
			return PartionsOfEquipmentPartial();
		}

		[ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentPartial()
		{
			if (GridViewHelper.SelectedEquipmentID < 0)
			{
				var model = new Equipment();
				return PartialView("_TechnicalDocumentOfEquipmentPartial", model);
			}
			else
			{
				//var equipments = IncosafCMS.Web.Providers.EquipmentDataProvider.Equipments.ToList();
				//var model = equipments.FirstOrDefault(x => x.Id == GridViewHelper.SelectedEquipmentID);
				//if (model?.TechnicalDocuments == null) model = new Equipment();
				var model = EquipmentDataProvider.GetTechnicalDocuments(GridViewHelper.SelectedEquipmentID);
				return PartialView("_TechnicalDocumentOfEquipmentPartial", model);
			}
		}
		public ActionResult CustomCallBackTechnicalDocumentOfEquipmentAction(int selectedequipment)
		{
			if (string.IsNullOrEmpty(selectedequipment.ToString()) || selectedequipment < 0) GridViewHelper.SelectedEquipmentID = -1;
			else GridViewHelper.SelectedEquipmentID = selectedequipment;
			return TechnicalDocumentOfEquipmentPartial();
		}

		#endregion

		#region Create View
		#region LoadTests Of Equipment
		[ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentCreatePartial()
		{
			return PartialView("_LoadTestsOfEquipmentCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
					{
						ViewData["EditableLoadTests"] = loadTest;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentLoadTestsList.AddLoadTests(loadTest);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableLoadTests"] = loadTest;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_LoadTestsOfEquipmentCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
					{
						ViewData["EditableLoadTests"] = loadTest;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentLoadTestsList.UpdateLoadTests(loadTest);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableLoadTests"] = loadTest;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_LoadTestsOfEquipmentCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentCreatePartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentLoadTestsList.DeleteLoadTests(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_LoadTestsOfEquipmentCreatePartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		#endregion
		#region Specifications Of Equipment
		[ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentCreatePartial()
		{
			return PartialView("_SpecificationsOfEquipmentCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
					{
						ViewData["EditableSpecifications"] = specifications;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentSpecificationsList.AddSpecifications(specifications);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableSpecifications"] = specifications;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_SpecificationsOfEquipmentCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
					{
						ViewData["EditableSpecifications"] = specifications;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentSpecificationsList.UpdateSpecifications(specifications);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableSpecifications"] = specifications;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_SpecificationsOfEquipmentCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentCreatePartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentSpecificationsList.DeleteSpecifications(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_SpecificationsOfEquipmentCreatePartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		#endregion
		//----------------------------------------//
		#region Partions Of Equipment
		[ValidateInput(false)]
		public ActionResult PartionsOfEquipmentCreatePartial()
		{
			return PartialView("_PartionsOfEquipmentCreatePartial", EquipmentPartionList.GetEquipmentPartion);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult PartionsOfEquipmentCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
					{
						ViewData["EditablePartion"] = equipmentPartion;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentPartionList.AddEquipmentPartion(equipmentPartion);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditablePartion"] = equipmentPartion;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_PartionsOfEquipmentCreatePartial", EquipmentPartionList.GetEquipmentPartion);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult PartionsOfEquipmentCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
					{
						ViewData["EditablePartion"] = equipmentPartion;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentPartionList.UpdateEquipmentPartion(equipmentPartion);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditablePartion"] = equipmentPartion;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_PartionsOfEquipmentCreatePartial", EquipmentPartionList.GetEquipmentPartion);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult PartionsOfEquipmentCreatePartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentPartionList.DeleteEquipmentPartion(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_PartionsOfEquipmentCreatePartial", EquipmentPartionList.GetEquipmentPartion);
		}
		#endregion
		//--------------------------------------------//
		#region TechnicalDocuments of Equipment
		[ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentCreatePartial()
		{
			return PartialView("_TechnicalDocumentOfEquipmentCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentCreatePartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(technicalDocument.Name))
					{
						ViewData["EditableTechnicalDocument"] = technicalDocument;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentTechnicalDocumentList.AddEquipmentTechnicalDocument(technicalDocument);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableTechnicalDocument"] = technicalDocument;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_TechnicalDocumentOfEquipmentCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentCreatePartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(technicalDocument.Name))
					{
						ViewData["EditableTechnicalDocument"] = technicalDocument;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentTechnicalDocumentList.UpdateEquipmentTechnicalDocument(technicalDocument);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditablePartion"] = technicalDocument;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_TechnicalDocumentOfEquipmentCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentCreatePartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentTechnicalDocumentList.DeleteEquipmentTechnicalDocument(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_TechnicalDocumentOfEquipmentCreatePartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		#endregion
		#endregion

		#region Edit View

		#region LoadTests Of Equipment
		[ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentEditPartial()
		{
			return PartialView("_LoadTestsOfEquipmentEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
					{
						ViewData["EditableLoadTests"] = loadTest;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentLoadTestsList.AddLoadTests(loadTest);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableLoadTests"] = loadTest;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_LoadTestsOfEquipmentEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] LoadTest loadTest)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(loadTest.LocalTest))
					{
						ViewData["EditableLoadTests"] = loadTest;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentLoadTestsList.UpdateLoadTests(loadTest);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableLoadTests"] = loadTest;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_LoadTestsOfEquipmentEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult LoadTestsOfEquipmentEditPartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentLoadTestsList.DeleteLoadTests(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_LoadTestsOfEquipmentEditPartial", EquipmentLoadTestsList.GetEquipmentLoadTests);
		}
		#endregion
		#region Specifications Of Equipment
		[ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentEditPartial()
		{            
			return PartialView("_SpecificationsOfEquipmentEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
					{
						ViewData["EditableSpecifications"] = specifications;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentSpecificationsList.AddSpecifications(specifications);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableSpecifications"] = specifications;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_SpecificationsOfEquipmentEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] Specifications specifications)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(specifications.Name) || string.IsNullOrWhiteSpace(specifications.Value))
					{
						ViewData["EditableSpecifications"] = specifications;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentSpecificationsList.UpdateSpecifications(specifications);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableSpecifications"] = specifications;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_SpecificationsOfEquipmentEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult SpecificationsOfEquipmentEditPartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentSpecificationsList.DeleteSpecifications(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_SpecificationsOfEquipmentEditPartial", EquipmentSpecificationsList.GetEquipmentSpecifications);
		}
		#endregion
		//----------------------------------------//
		#region Partions Of Equipment
		[ValidateInput(false)]
		public ActionResult PartionsOfEquipmentEditPartial()
		{
			return PartialView("_PartionsOfEquipmentEditPartial", EquipmentPartionList.GetEquipmentPartion);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult PartionsOfEquipmentEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
					{
						ViewData["EditablePartion"] = equipmentPartion;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentPartionList.AddEquipmentPartion(equipmentPartion);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditablePartion"] = equipmentPartion;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_PartionsOfEquipmentEditPartial", EquipmentPartionList.GetEquipmentPartion);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult PartionsOfEquipmentEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] EquipmentPartion equipmentPartion)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(equipmentPartion.Name))
					{
						ViewData["EditablePartion"] = equipmentPartion;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentPartionList.UpdateEquipmentPartion(equipmentPartion);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditablePartion"] = equipmentPartion;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_PartionsOfEquipmentEditPartial", EquipmentPartionList.GetEquipmentPartion);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult PartionsOfEquipmentEditPartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentPartionList.DeleteEquipmentPartion(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_PartionsOfEquipmentEditPartial", EquipmentPartionList.GetEquipmentPartion);
		}
		#endregion
		//--------------------------------------//
		#region TechnicalDocuments Of Equipment
		[ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentEditPartial()
		{
			return PartialView("_TechnicalDocumentOfEquipmentEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentEditPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(technicalDocument.Name))
					{
						ViewData["EditableTechnicalDocument"] = technicalDocument;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentTechnicalDocumentList.AddEquipmentTechnicalDocument(technicalDocument);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableTechnicalDocument"] = technicalDocument;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_TechnicalDocumentOfEquipmentEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentEditPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] TechnicalDocument technicalDocument)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (string.IsNullOrWhiteSpace(technicalDocument.Name))
					{
						ViewData["EditableTechnicalDocument"] = technicalDocument;
						ViewData["EditError"] = "Đã có lỗi xảy ra. Các trường không được bỏ trống.";
					}
					else
						EquipmentTechnicalDocumentList.UpdateEquipmentTechnicalDocument(technicalDocument);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
					ViewData["EditableTechnicalDocument"] = technicalDocument;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("_TechnicalDocumentOfEquipmentEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult TechnicalDocumentOfEquipmentEditPartialDelete(System.Int32 Id)
		{
			if (Id >= 0)
			{
				try
				{
					EquipmentTechnicalDocumentList.DeleteEquipmentTechnicalDocument(Id);
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("_TechnicalDocumentOfEquipmentEditPartial", EquipmentTechnicalDocumentList.GetEquipmentTechnicalDocument);
		}
		#endregion
		#endregion

		#region Create From Lib
		// GET: Equipments/Create
		public ActionResult CreateFromLib()
		{
			ViewData["Equipments"] = uow.Repository<OriginalEquipment>().GetAll();
			//ViewData["Contracts"] = User.IsInRole("KDV") ? uow.Repository<Contract>().GetAll().Where(x => x.own?.UserName == User.Identity.Name && x.Status == ApproveStatus.Waiting) : uow.Repository<Contract>().GetAll().Where(x => x.Status == ApproveStatus.Waiting);

			if (User.IsInRole("Admin"))
			{
				ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).ToList();
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user != null)
				{
					if (user.Department != null)
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).Where(x => x.DepartmentId == user.Department.Id).ToList();
					else
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(user.Id).ToList();
				}
				else
					ViewData["Contracts"] = new List<ContractViewModel>();
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Contracts"] = user != null ? ContractDataProvider.GetContractsByOwnerID(user.Id).ToList() : new List<ContractViewModel>();
			}
			return View();
		}

		// POST: Equipments/Create
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateFromLib(FormCollection collection)
		{
			try
			{
				var strOriginalEquipment = EditorExtension.GetValue<string>("cmbOriginalEquipment");
				int originalEquipmentID = 0;
				if (int.TryParse(strOriginalEquipment, out originalEquipmentID))
				{
					var originalEquipment = uow.Repository<OriginalEquipment>().GetSingle(originalEquipmentID);

					var strContractID = EditorExtension.GetValue<string>("cmbContractCreateFromLib");
					int contractID = 0;
					if (int.TryParse(strContractID, out contractID))
					{
						var contract = uow.Repository<Contract>().GetSingle(contractID);

						var strAmount = EditorExtension.GetValue<string>("speAmountCreateFromLib");
						int amount = 1;
						if (!int.TryParse(strAmount, out amount)) amount = 1;

						var strPrice = EditorExtension.GetValue<string>("spePriceCreateFromLib");
						double price = 0;
						if (!double.TryParse(strPrice, out price)) price = 0;

						var strTaskID = EditorExtension.GetValue<string>("cmbTaskOfContractCreateFromLib");
						int taskID = 0;
						if (int.TryParse(strTaskID, out taskID) && taskID > 0)
						{
							var tenTB = EditorExtension.GetValue<string>("txtTenThietBi");
							var accTask = contract.Tasks.Where(x => x.Id == taskID).FirstOrDefault();
							if (accTask != null)
							{
								for (int i = 0; i < amount; i++)
								{
									// Thêm thiết bị
									var newEquipment = new Equipment()
									{
										Id = 0,
										Name = string.IsNullOrWhiteSpace(tenTB) ? originalEquipment.Name : tenTB,
										Code = originalEquipment.Code,
										No = originalEquipment.No,
										YearOfProduction = originalEquipment.YearOfProduction,
										AccrPeriod = originalEquipment.AccrPeriod, // Added by lapbt 08-jun-2023. Sử dụng để tính DateOfNext thay cho trường YearOfProduction
										ManuFacturer = originalEquipment.ManuFacturer,
										Uses = originalEquipment.Uses,
										isPrintGcn = false, //Hưng thêm 09.06.2025
										contract = contract
									};

									newEquipment.contract.customer = contract.customer;

									foreach (var partion in originalEquipment.Partions)
									{
										var newPartion = new EquipmentPartion()
										{
											Id = 0,
											Name = partion.Name,
											Note = partion.Note,
											Passed1 = true,
											Passed2 = partion.Passed2
										};

										newEquipment.Partions.Add(newPartion);

										uow.Repository<EquipmentPartion>().Insert(newPartion);
									}

									//loadTest - 08.06.2025 đóng do chưa dùng
									//foreach (var loadTest in originalEquipment.LoadTests)
									//{
									//	var newLoadTest = new LoadTest()
									//	{
									//		Id = 0,
									//		CorrespondingLoad = loadTest.CorrespondingLoad,
									//		DynamicLoad = loadTest.DynamicLoad,
									//		LocalTest = loadTest.LocalTest,
									//		Passed = true,
									//		Radius = loadTest.Radius,
									//		StaticLoad = loadTest.StaticLoad
									//	};

									//	newEquipment.LoadTests.Add(newLoadTest);

									//	uow.Repository<LoadTest>().Insert(newLoadTest);
									//}

									//24.05.2025									
									foreach (var specification in originalEquipment.specifications)
									{
										var newSpecification = new Specifications()
										{
											Id = 0,
											Name = specification.Name,
											Value = specification.Value,
											f_key = specification.f_key,
											f_unit = specification.f_unit
										};

										newEquipment.specifications.Add(newSpecification);

										uow.Repository<Specifications>().Insert(newSpecification);
									}

									//technicaldocument - 08.06.2025 đóng do chưa dùng
									//foreach (var technicaldocument in originalEquipment.TechnicalDocuments)
									//{
									//	var newTechnicaldocument = new TechnicalDocument()
									//	{
									//		Id = 0,
									//		Name = technicaldocument.Name,
									//		Note = technicaldocument.Note,
									//		Passed = true
									//	};

									//	newEquipment.TechnicalDocuments.Add(newTechnicaldocument);

									//	uow.Repository<TechnicalDocument>().Insert(newTechnicaldocument);
									//}

									// Thêm thông tin kiểm định vào hợp đồng
									string dept = string.IsNullOrEmpty(accTask.AccTaskNote) ? "TBN" : accTask.AccTaskNote.Split('-').Length == 2 ? accTask.AccTaskNote.Split('-')[1] : "TBN";
									var prefix = DateTime.Today.ToString("yy") + ".";

									var autoAccreNumber = DataProvider.DB.Database.SqlQuery<AutoAccreNumber>("GetLastAutoAccreNumber @dept, @prefix", new SqlParameter("@dept", dept), new SqlParameter("@prefix", prefix)).FirstOrDefault();

									AutoAccreNumber nextAccreNumber = null;
									if (autoAccreNumber == null || autoAccreNumber.Prefix != prefix)
									{
										nextAccreNumber = new AutoAccreNumber()
										{
											Id = 0,
											Prefix = DateTime.Today.ToString("yy") + ".",
											Dept = dept,
											AccreNumber = 1
										};

										uow.Repository<AutoAccreNumber>().Insert(nextAccreNumber);
										uow.SaveChanges();
									}
									else
									{
										nextAccreNumber = new AutoAccreNumber()
										{
											Id = 0,
											Prefix = DateTime.Today.ToString("yy") + ".",
											Dept = dept,
											AccreNumber = autoAccreNumber.AccreNumber + 1
										};

										uow.Repository<AutoAccreNumber>().Insert(nextAccreNumber);
										uow.SaveChanges();
									}

									/* Added by lapbt
									 * 28-oct-2021. ô "Hạn kiểm định" sẽ có giá trị mặc định là "Ngày kiểm định" + YearOfProduction (năm)
									 * 08-jun-2023. Sử dụng AccrPeriod để tính DateOfNext thay cho trường YearOfProduction
									 */
									//if (!int.TryParse(originalEquipment.YearOfProduction, out int yearOfNextAccr))
									//{
									//    yearOfNextAccr = 1;   // default if not set
									//}
									int yearOfNextAccr = Convert.ToInt32(originalEquipment.AccrPeriod);
									yearOfNextAccr = (yearOfNextAccr >= 0 && yearOfNextAccr < 10) ? yearOfNextAccr : 1;

									var newAccreditation = new Accreditation()
									{
										Id = 0,
										equiment = newEquipment,
										NumberAcc = nextAccreNumber.AccreNumber.ToString("#00000"),
										AccrDate = contract.NgayThucHien ?? DateTime.Today,
										//DateOfNext = contract.NgayThucHien.HasValue ? contract.NgayThucHien.Value.AddYears(1) : DateTime.Today.AddYears(1),
										DateOfNext = contract.NgayThucHien.HasValue ? contract.NgayThucHien.Value.AddYears(yearOfNextAccr) : DateTime.Today.AddYears(yearOfNextAccr),
										AccrResultDate = contract.NgayThucHien ?? DateTime.Today,
									};

									accTask.Accreditations.Add(newAccreditation);
									if (accTask.Amount < accTask.Accreditations.Count) accTask.Amount = accTask.Accreditations.Count;
									uow.Repository<Accreditation>().Insert(newAccreditation);

									accTask.UnitPrice = price;
									uow.Repository<AccTask>().Update(accTask);
									service.Add(newEquipment);

									contract.Value = contract.Tasks.Sum(x => x.Amount * x.UnitPrice);
									uow.Repository<Contract>().Update(contract);
									uow.SaveChanges();
								}
							}
						}
						else
						{
							//var strUnit = EditorExtension.GetValue<string>("txtUnitCreateFromLib");
							//var strAccTaskName = EditorExtension.GetValue<string>("txtTaskNameCreateFromLib");

							//var newAccTask = new AccTask()
							//{
							//    Name = strAccTaskName,
							//    Unit = strUnit,
							//    UnitPrice = price,
							//    Amount = amount
							//};

							//for (int i = 0; i < amount; i++)
							//{
							//    // Thêm thiết bị
							//    var newEquipment = new Equipment()
							//    {
							//        Id = 0,
							//        Name = originalEquipment.Name,
							//        Code = originalEquipment.Code,
							//        No = originalEquipment.No,
							//        YearOfProduction = originalEquipment.YearOfProduction,
							//        ManuFacturer = originalEquipment.ManuFacturer,
							//        Uses = originalEquipment.Uses,
							//        contract = contract
							//    };

							//    newEquipment.contract.customer = contract.customer;

							//    foreach (var partion in originalEquipment.Partions)
							//    {
							//        var newPartion = new EquipmentPartion()
							//        {
							//            Id = 0,
							//            Name = partion.Name,
							//            Note = partion.Note,
							//            Passed1 = true,
							//            Passed2 = partion.Passed2
							//        };

							//        newEquipment.Partions.Add(newPartion);

							//        uow.Repository<EquipmentPartion>().Insert(newPartion);
							//    }

							//    foreach (var loadTest in originalEquipment.LoadTests)
							//    {
							//        var newLoadTest = new LoadTest()
							//        {
							//            Id = 0,
							//            CorrespondingLoad = loadTest.CorrespondingLoad,
							//            DynamicLoad = loadTest.DynamicLoad,
							//            LocalTest = loadTest.LocalTest,
							//            Passed = true,
							//            Radius = loadTest.Radius,
							//            StaticLoad = loadTest.StaticLoad
							//        };

							//        newEquipment.LoadTests.Add(newLoadTest);

							//        uow.Repository<LoadTest>().Insert(newLoadTest);
							//    }

							//    foreach (var specification in originalEquipment.specifications)
							//    {
							//        var newSpecification = new Specifications()
							//        {
							//            Id = 0,
							//            Name = specification.Name,
							//            Value = specification.Value
							//        };

							//        newEquipment.specifications.Add(newSpecification);

							//        uow.Repository<Specifications>().Insert(newSpecification);
							//    }

							//    foreach (var technicaldocument in originalEquipment.TechnicalDocuments)
							//    {
							//        var newTechnicaldocument = new TechnicalDocument()
							//        {
							//            Id = 0,
							//            Name = technicaldocument.Name,
							//            Note = technicaldocument.Note,
							//            Passed = true
							//        };

							//        newEquipment.TechnicalDocuments.Add(newTechnicaldocument);

							//        uow.Repository<TechnicalDocument>().Insert(newTechnicaldocument);
							//    }

							//    // Thêm thông tin kiểm định vào hợp đồng

							//    var newAccreditation = new Accreditation()
							//    {
							//        Id = 0,
							//        equiment = newEquipment,
							//NumberAcc = nextAccreNumber.AccreNumber.ToString("#00000"),
							//AccrDate = contract.NgayThucHien ?? DateTime.Today,
							//DateOfNext = contract.NgayThucHien.HasValue ? contract.NgayThucHien.Value.AddYears(1) : DateTime.Today.AddYears(1),
							//AccrResultDate = contract.NgayThucHien ?? DateTime.Today,
							//    };

							//    newAccTask.Accreditations.Add(newAccreditation);
							//    uow.Repository<Accreditation>().Insert(newAccreditation);

							//    service.Add(newEquipment);
							//}

							//contract.Tasks.Add(newAccTask);
							//contract.Value = contract.Tasks.Sum(x => x.Amount * x.UnitPrice);
							//uow.Repository<Contract>().Update(contract);
							//uow.SaveChanges();
						}
					}
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}


			return RedirectToAction("Equipment", "Home");
		}

		/*
		 * Note by Lapbt
		 * Sự kiện: Click (b1) vào nút "Tạo TB" ở panel bên phải trong nội dung CV 
		 * Vđ: đg dò lỗi k0 tạo đc số BBKD, với t.hợp sau khi tạo t.bi xong, nhg k0 reload đc ds cv bên phải
		 * 11-02-2023
		 *  - Dò yc khi Chọn loại thiết bị từ thư viện, cần ktra xem OriginalEquipment.AccreditorType == 'BXD' thì ở ô số lượng bên dưới chỉ cho max = 1
		 *  - Mô tả cách build combobox như sau
		 *      - Dòng dưới ViewData["Equipments"] = uow.Repository<OriginalEquipment>().GetAll(); lấy toàn bộ ds thiết bị về (mk quá nặng)
		 *      - Sau đó trả về cho view CreateFromLibViaContract -> 
		 *        lại gọi tới hàm IncosafCMS.Web.Controllers.EquipmentsController.CreateTreeViewNodesRecursive(equipments, settingsTV.Nodes, acctaskNode: (string)ViewData["AccTaskNode"]); 
		 *        để lúc đó mới lọc để chỉ lấy loại thiết bị trong công việc ViewData["AccTaskNode"]
		 *      - Lọc rồi build lại để gửi trả về view
		 *  - Trong khi mỗi thiết bị lại có thể có AccreditorType khác nhau, nên chỉ khi chọn xong vào t.bị nào thì mới biết nó có nằm trong 'BXD' hay k0, 
		 *    từ đó mới chỉnh ô số lượng là 1 hay để nguyên!!!
		 *  - Cần sửa cả modal OriginalEquipment, vì chưa có trường này, do trường này mới thêm về sau
		 *  => Ktra khi thực hiện Post, chứ ở ngay dưới thì fai dùng js và k0 có dữ liệu
		 *  
		 * 11-05-2024
		 * Bổ sung thêm các trường hợp không cho lấy số KQKĐ, khi bấm nút “Tạo TB” sẽ cảnh báo và không cho lấy số, ngoại trừ quyền admin vẫn cho lấy số:
		 *   + HĐ chưa cấp số (MaHD=NULL) và có ngày khởi tạo (CreateDate) > 30 ngày so với hiện hành
		 *   + HĐ đã cấp số (MaHD<>NULL) và có ngày khởi tạo (CreateDate) > 180 ngày so với hiện hành
		 *   + HĐ đã kết thúc (trong bảng Contract, cột Finished = 1)		 
		 */

		public ActionResult CreateFromLibViaContract(int ContractID, int TaskID)
		{
			var contract = uow.Repository<Contract>().FindBy(x => x.Id == ContractID).FirstOrDefault();
			var task = contract?.Tasks.FirstOrDefault(x => x.Id == TaskID);		

			if (contract != null && task != null)
			{
				// edited by lapbt 6-jan-2021. Chi count cac thiet da cap so
				var accs = uow.Repository<Accreditation>().FindBy(e => e.AccTask_Id == TaskID && string.IsNullOrEmpty(e.NumberAcc));
				
				if (accs.Count > 0)
				{
					var del_null = false;
					foreach (var a in accs)
					{
						// Xoa cac dong co NumberAcc == Null
						if (a.NumberAcc == null)
						{
							System.Diagnostics.Debug.WriteLine(a.Id);
							System.Diagnostics.Debug.WriteLine(a.NumberAcc);
							uow.Repository<Accreditation>().Delete(a);
							del_null = true;
						}
					}
					
					if (del_null)
					{
						uow.SaveChanges();
						task = contract?.Tasks.FirstOrDefault(x => x.Id == TaskID);
					}
				}

				// --- Thêm xử lý xóa thiết bị không còn liên kết ---
				// Lấy toàn bộ AccTask.Id thuộc contract hiện tại				
				var accTaskIds = contract?.Tasks.Select(t => t.Id).ToList();
				// Lấy toàn bộ equipment_Id trong bảng Accreditation ứng với các accTaskIds đó
				var accEquipIds = uow.Repository<Accreditation>()
					.FindBy(e => accTaskIds.Contains(e.AccTask.Id))
					.Select(e => e.equiment.Id)
					.ToList();

				// Lấy thiết bị thuộc contract nhưng không có trong danh sách accEquipIds
				var equipmentsToDelete = uow.Repository<Equipment>()
					.FindBy(eq => eq.contract.Id == ContractID && !accEquipIds.Contains(eq.Id))
					.ToList();

				if (equipmentsToDelete.Any())
				{
					foreach (var eq in equipmentsToDelete)
					{
						// 🔹 Xóa tất cả Specifications trước
						var specs = uow.Repository<Specifications>()
							.FindBy(s => s.Equipment.Id == eq.Id)
							.ToList();

						foreach (var sp in specs)
						{
							uow.Repository<Specifications>().Delete(sp);
						}

						// 🔹 Sau đó mới xóa Equipment
						uow.Repository<Equipment>().Delete(eq);
					}
					uow.SaveChanges();
				}
				// Cảnh báo và không cho lấy số, ngoại trừ quyền admin vẫn cho lấy số
				if (!User.IsInRole("Admin"))
				{
					// Calculate the difference in days between the two dates
					TimeSpan difference = DateTime.Today - contract.CreateDate;
					TimeSpan difference1;
					if (contract.NgayThucHienDen != null)
					{
						difference1 = DateTime.Today - contract.NgayThucHienDen.Value;
					}
					else
					{
						difference1 = DateTime.Today - contract.NgayThucHien.Value;
					}
					if (string.IsNullOrEmpty(contract.MaHD) && difference.TotalDays > 60) //Hưng sửa - 30 ngày tăng lên 60 ngày
					{
						// +HĐ chưa cấp số(MaHD = NULL) và có ngày khởi tạo(CreateDate) > 60 ngày so với hiện hành
						return Json("false_1", JsonRequestBehavior.AllowGet);
					} else if (!string.IsNullOrEmpty(contract.MaHD) && difference.TotalDays > 365 && difference1.TotalDays > 30)
					{
						// +HĐ đã cấp số(MaHD <> NULL) và có ngày khởi tạo(CreateDate) > 365 ngày và Ngày thực hiện đến > 30 ngày so với hiện hành
						return Json("false_2", JsonRequestBehavior.AllowGet);
					} else if (contract.Finished)
					{
						// +HĐ đã kết thúc(trong bảng Contract, cột Finished = 1)
						return Json("false_3", JsonRequestBehavior.AllowGet);
					}
				}
				//Hưng thêm 6.3.2025 - Cảnh báo Chưa gán loại hình công việc vào đối tượng lấy số KQ
				if (task.AccTaskNote == null)
				{
					return Json("nullAccTaskNote", JsonRequestBehavior.AllowGet);
					//editContract()
				}

				if (task.Accreditations.Count >= task.Amount)
				{
					return Json("overCount", JsonRequestBehavior.AllowGet);
				}
				else
				{
					ViewData["Equipments"] = uow.Repository<OriginalEquipment>().GetAll();
					ViewData["ContractID"] = ContractID;
					ViewData["TaskID"] = TaskID;
					ViewData["AccTaskNode"] = task?.AccTaskNote;
					ViewData["ContractName"] = contract?.Name;
					ViewData["TaskName"] = task?.Name;
					ViewData["MinCount"] = task.Accreditations.Count < task.Amount ? 1 : 0;
					ViewData["MaxCount"] = task.Amount - task.Accreditations.Count;
					return View();
				}
			}
			else
			{
				return Json("false", JsonRequestBehavior.AllowGet);
			}
		}

		/*
		 * Note by Lapbt
		 *  - Trc khi post thì sdung Script\app\index.js để ktra dữ liệu trc
		 * Event: sau khi click (b2) vào nút "Tạo T.B" bên phải -> lên form để tạo mới thiết bị -> chọn thiết bị -> Nhấn "Tạo thiết bị" sẽ post tới đây 
		 *      - Có vđ chưa tìm ra ở đây, là sau khi nhấn OK kết thúc tạo mới -> Sẽ tạo thành công ok thì 
		 *          + chỗ nào để gọi tiếp việc show lên form để lấy số??
		 *          + và đồng thời cũng có vc reload lại ds cv ở đây?? chính chỗ reload k0 thành công nên có thể là lấy số k0 đc!!!???
		 * Ndung xly: dò lỗi k0 cấp đc số BBKD sau khi Tạo T.B
		 * 11-02-2023
		 *  - Code thêm ktra nếu AccreditorType == 'BXD' thì số lượng thiết bị chỉ cho = 1
		 */
		/* 9.9.2025 thay toàn bộ code mới cho hàm bên dưới để tối ưu tốc độ
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateFromLibViaContract(int ContractID, int TaskID, string originalEquipmentName, int originalEquipmentID, int Count)
		{			
			EquipmentAddedList.EquipmentsAdded = new List<Equipment>();
			try
			{				
				var splts = originalEquipmentName.Split(new string[] {" - " }, StringSplitOptions.RemoveEmptyEntries);

				if (splts?.Count() == 2)				
				{					
					var code = splts[0];
					var name = splts[1];
					var originalEquipment = uow.Repository<OriginalEquipment>().GetSingle(e => e.Code == code && e.Name == name);

					if (originalEquipment != null)
					{
						if (ContractID > 0)
						{
							var contract = uow.Repository<Contract>().GetSingle(ContractID);
							if (contract != null)
							{
								if (TaskID > 0)
								{
									var accTask = contract.Tasks.FirstOrDefault(x => x.Id == TaskID);
									if (accTask != null)
									{										
										var lstNewEquipment = new List<Equipment>();
										for (int i = 0; i < Count; i++)
										{
											// Thêm thiết bị
											var newEquipment = new Equipment()
											{
												Id = 0,
												Name = originalEquipment.Name,
												Code = originalEquipment.Code,												
												AccrPeriod = originalEquipment.AccrPeriod,  // Added by lapbt 08-jun-2023. Sử dụng để tính DateOfNext thay cho trường YearOfProduction												
												isPrintGcn = false, //Hưng thêm 09.06.2025
												contract = contract
											};

											newEquipment.contract.customer = contract.customer;

											//24.05.2025 Khi Create TB mới (specifications.Count = 0) thì load TSKT từ TB mẫu
											if (originalEquipment.ParentCode == "TBN" || originalEquipment.ParentCode == "TBAL")
											{	
												var spec = uow.Repository<Equipment>().GetSingle(e => e.mahieu == newEquipment.Code.ToString() + ".0000");												
												if (spec != null)
                                                {
													foreach (var specification in spec.specifications)
													{
														var newSpecification = new Specifications()
														{
															Id = 0,
															Name = specification.Name,
															Value = specification.Value,
															f_key = specification.f_key,
															f_unit = specification.f_unit
														};
														newEquipment.specifications.Add(newSpecification);
														uow.Repository<Specifications>().Insert(newSpecification);														
													}
												}
											}

											int yearOfNextAccr = Convert.ToInt32(originalEquipment.AccrPeriod);
											yearOfNextAccr = (yearOfNextAccr >= 0 && yearOfNextAccr < 10) ? yearOfNextAccr : 1;

											var newAccreditation = new Accreditation()
											{
												Id = 0,
												equiment = newEquipment,												
												AccrDate = contract.NgayThucHien ?? DateTime.Today,												
												DateOfNext = contract.NgayThucHien.HasValue ? contract.NgayThucHien.Value.AddYears(yearOfNextAccr) : DateTime.Today.AddYears(yearOfNextAccr),
												AccrResultDate = contract.NgayThucHien ?? DateTime.Today,
											};
											
											accTask.Accreditations.Add(newAccreditation);											
											uow.Repository<Accreditation>().Insert(newAccreditation);											
											service.Add(newEquipment);

											lstNewEquipment.Add(newEquipment);
										}
										
										uow.SaveChanges();
										EquipmentAddedList.EquipmentsAdded = lstNewEquipment;
										return Json(new object[] { "success", lstNewEquipment.First().Id }, JsonRequestBehavior.AllowGet);
										
									}
								}
							}
						}
					}
				}

				return Json(new object[] { "error", -1 }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{				
				ViewData["EditError"] = e.Message;
				return Json(new object[] { "error", -1 }, JsonRequestBehavior.AllowGet);
			}
		}
		*/


		[HttpPost, ValidateInput(false)]
		public ActionResult CreateFromLibViaContract(int ContractID, int TaskID, string originalEquipmentName, int originalEquipmentID, int Count)
		{
			EquipmentAddedList.EquipmentsAdded = new List<Equipment>();
			try
			{
				var splts = originalEquipmentName.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
				if (splts?.Length == 2)
				{
					var code = splts[0];
					var name = splts[1];
					var originalEquipment = uow.Repository<OriginalEquipment>()
											   .GetSingle(e => e.Code == code && e.Name == name);

					if (originalEquipment != null && ContractID > 0)
					{
						var contract = uow.Repository<Contract>().GetSingle(ContractID);
						if (contract != null && TaskID > 0)
						{
							var accTask = contract.Tasks.FirstOrDefault(x => x.Id == TaskID);
							if (accTask != null)
							{
								var lstNewEquipment = new List<Equipment>();

								// 24.9.2025 Load spec mẫu qua bảng OriginalSpec
								var templateSpecs = uow.Repository<OriginalSpec>()
													.FindBy(os => os.OriginalEquipment_Code != null
															&& os.OriginalEquipment_Code == code)
													.ToList();

								/* Code cũ, lấy Spec qua mahieu = code.0000
								List<Specifications> templateSpecs = new List<Specifications>();
								if (originalEquipment.ParentCode == "TBN" || originalEquipment.ParentCode == "TBAL")
								{
									var specEquip = uow.Repository<Equipment>().GetSingle(e => e.mahieu == code + ".0000");
									if (specEquip != null)
										templateSpecs = specEquip.specifications.ToList();

																		
								}
								*/
								int yearOfNextAccr = Convert.ToInt32(originalEquipment.AccrPeriod);
								yearOfNextAccr = (yearOfNextAccr >= 0 && yearOfNextAccr < 10) ? yearOfNextAccr : 1;

								for (int i = 0; i < Count; i++)
								{
									var newEquipment = new Equipment()
									{
										Name = originalEquipment.Name,
										Code = originalEquipment.Code,
										AccrPeriod = originalEquipment.AccrPeriod,
										isPrintGcn = false,
										contract = contract,
									};

									newEquipment.contract.customer = contract.customer;

									// 🔹 Clone spec từ template
									if (templateSpecs.Count > 0)
										foreach (var s in templateSpecs)
										{
											newEquipment.specifications.Add(new Specifications
											{
												Name = s.Name,
												Value = "",
												f_key = s.f_key,
												f_unit = s.f_unit
											});
										}

									var newAccreditation = new Accreditation()
									{
										equiment = newEquipment,
										AccrDate = contract.NgayThucHien ?? DateTime.Today,
										DateOfNext = contract.NgayThucHien.HasValue
											? contract.NgayThucHien.Value.AddYears(yearOfNextAccr)
											: DateTime.Today.AddYears(yearOfNextAccr),
										AccrResultDate = contract.NgayThucHien ?? DateTime.Today,
									};

									accTask.Accreditations.Add(newAccreditation);
									lstNewEquipment.Add(newEquipment);
									service.Add(newEquipment); // gắn vào DbSet
								}

								// 🔹 Chỉ SaveChanges 1 lần
								uow.SaveChanges();

								EquipmentAddedList.EquipmentsAdded = lstNewEquipment;
								return Json(new object[] { "success", lstNewEquipment.First().Id }, JsonRequestBehavior.AllowGet);
							}
						}
					}
				}

				return Json(new object[] { "error", -1 }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
				return Json(new object[] { "error", -1 }, JsonRequestBehavior.AllowGet);
			}
		}


		public ActionResult SelectEquipmentForEditAccreditation(int ContractID, int TaskID)
		{   
			var contract = uow.Repository<Contract>().FindBy(x => x.Id == ContractID).FirstOrDefault();
			var task = contract?.Tasks.FirstOrDefault(x => x.Id == TaskID);

			//05.06.2025 cho phép kđv sửa BBKĐ trong vòng 60 ngày sau ngày khởi tạo khi Hdong đã cấp số (Admin vẫn sửa all)

			TimeSpan difference = DateTime.Today - contract.CreateDate;

			if (contract != null && task != null && 
			(string.IsNullOrEmpty(contract.MaHD) || !string.IsNullOrEmpty(contract.MaHD) && difference.TotalDays > 60 || User.IsInRole("Admin")))
			{
				// edited by lapbt 06-jun-2021
				/**
				 * Lưu lại cách xử lý chỗ này như sau
				 * 1. Thực trạng
				 * - Code cũ sẽ trả về list các obj <Equipment> đã đc lọc từ Accreditations
				 * - Tuy nhiên model này chỉ có các trường của thiết bị đơn thuần, nên muốn hiển thêm số KQKĐ thì model Equip lại k0 có
				 * - Nếu lấy ở model Accreditation, thì sẽ có số KQKĐ, nhưng lại k0 có ID, Name của Equipment
				 * 
				 * 2. Nguyên nhân
				 * - Do DL nằm ở cả 2 bảng ~ 2 model Accreditation & Equipment --> Cần fai join lại
				 * 
				 * 3. Giải pháp
				 * - Cần SL có join giữa 2 bảng này --> chk thấy --> lấy theo model EquipmentViewModel thì có đủ DL join
				 * - Tuy nhiên ở model này chưa có trường AccTaskId (vì cần lọc theo nó) nên sửa 2 chỗ bên dưới
				 * - (1) ALTER PROCEDURE [dbo].[GetAllEquipments]  --> thêm trường acct.Id as AccTaskId,	--- added by lapbt 06-jun-2021
				 * - (2) Add EquipmentViewModel.cs thêm thuộc tính --> public int AccTaskId { get; set; }
				 * - Số liệu đc query từ EquipmentDataProvider.AllEquipments --> Chỗ này k0 tối ưu!!! Vì đi bốc nguyên cả bảng vào, quá nặng (tuy nhiên cg có cache)
				 * - Số liệu all sẽ lọc theo --> Where(x => x.AccTaskId == TaskID) --> Giá trị đc trả về vào biến mới để k0 ảnh hưởng
				 * - Sửa SelectEquipmentForEditAccreditation.cshtml --> Đổi sang model EquipmentViewModel để hiển thị thêm đc 1 cột AccreResultNumber
				 */

											// edited by lapbt 13-apr-2025
											/*
											 * Sửa lại bỏ cách dùng proc rất chậm. Thêm các thông tin của equipment chỉ view vào modal Accreditaion
											 * Cách 1 & 2 đều ép về modal Equipment. Cách 3 về Accreditaion, nên tên trường sẽ đổi ở trong SelectEquipmentForEditAccreditation.cshtml
											 */

											//ViewData["Equipments"] = task.Accreditations.Count > 0? task.Accreditations.Select(x => x.equiment).ToList(): new List<Equipment>();    // old code 1
											//ViewData["LapEquipments"] = task.Accreditations.Count > 0 ? EquipmentDataProvider.AllEquipments.Where(x => x.AccTaskId == TaskID).ToList() : new List<EquipmentViewModel>();    // new code by lapbt, old code 2, chậm quá
											ViewData["LapAccreditations"] = task.Accreditations.Count > 0 ? uow.Repository<Accreditation>().FindBy(x => x.AccTask_Id == TaskID) : new List<Accreditation>();
				ViewData["ContractID"] = ContractID;
				ViewData["TaskID"] = TaskID;
				ViewData["ContractName"] = contract?.Name;
				ViewData["TaskName"] = task?.Name;
				return View();
			}
			else
			{
				return Json("false", JsonRequestBehavior.AllowGet);
			}
		}
		public ActionResult SelectEquipmentForPrintAccreditation(int ContractID, int TaskID)
		{
			var contract = uow.Repository<Contract>().FindBy(x => x.Id == ContractID).FirstOrDefault();
			var task = contract?.Tasks.FirstOrDefault(x => x.Id == TaskID);

			if (contract != null && task != null && (task.AccTaskNote == "KĐXD-TBN" || task.AccTaskNote == "KĐXD-TBAL"))
			{

				ViewData["Equipments"] = task.Accreditations.Select(x => x.equiment).ToList();
				ViewData["ContractID"] = ContractID;
				ViewData["TaskID"] = TaskID;
				ViewData["ContractName"] = contract?.Name;
				ViewData["TaskName"] = task?.Name;
				return View();
			}
			else
			{
				return Json("THIẾT BỊ KHÔNG HỖ TRỢ MẪU IN - CHỈ IN GCN A5 VỚI THIẾT BỊ KĐAT", JsonRequestBehavior.AllowGet);
				//return Json("false", JsonRequestBehavior.AllowGet);
			}
		}
		public ActionResult GetAllContractForCreateFromLib()
		{
			if (User.IsInRole("Admin"))
			{
				ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).ToList();
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user != null)
				{
					if (user.Department != null)
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).Where(x => x.DepartmentId == user.Department.Id).ToList();
					else
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(user.Id).ToList();
				}
				else
					ViewData["Contracts"] = new List<ContractViewModel>();
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Contracts"] = user != null ? ContractDataProvider.GetContractsByOwnerID(user.Id).ToList() : new List<ContractViewModel>();
			}
			//ViewData["Contracts"] = User.IsInRole("KDV") ? uow.Repository<Contract>().GetAll().Where(x => x.own?.UserName == User.Identity.Name && x.Status == ApproveStatus.Waiting) : uow.Repository<Contract>().GetAll().Where(x => x.Status == ApproveStatus.Waiting);
			return PartialView();
		}
		#endregion
		public ActionResult GetAllContractForEdit()
		{
			//ViewData["Contracts"] = User.IsInRole("KDV") ? uow.Repository<Contract>().GetAll().Where(x => x.own?.UserName == User.Identity.Name) : uow.Repository<Contract>().GetAll();
			if (User.IsInRole("Admin"))
			{
				ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).ToList();
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user != null)
				{
					if (user.Department != null)
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).Where(x => x.DepartmentId == user.Department.Id).ToList();
					else
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(user.Id).ToList();
				}
				else
					ViewData["Contracts"] = new List<ContractViewModel>();
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Contracts"] = user != null ? ContractDataProvider.GetContractsByOwnerID(user.Id).ToList() : new List<ContractViewModel>();
			}
			return PartialView();
		}
		public ActionResult GetAllContractForCreate()
		{
			//ViewData["Contracts"] = User.IsInRole("KDV") ? uow.Repository<Contract>().GetAll().Where(x => x.own?.UserName == User.Identity.Name && x.Status == ApproveStatus.Waiting) : uow.Repository<Contract>().GetAll().Where(x => x.Status == ApproveStatus.Waiting);
			if (User.IsInRole("Admin"))
			{
				ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).ToList();
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user != null)
				{
					if (user.Department != null)
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(null).Where(x => x.DepartmentId == user.Department.Id).ToList();
					else
						ViewData["Contracts"] = ContractDataProvider.GetContractsByOwnerID(user.Id).ToList();
				}
				else
					ViewData["Contracts"] = new List<ContractViewModel>();
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Contracts"] = user != null ? ContractDataProvider.GetContractsByOwnerID(user.Id).ToList() : new List<ContractViewModel>();
			}
			return PartialView();
		}
		public ActionResult GetTaskOfContractCreate()
		{
			//var model = uow.Repository<AccTaskLib>().GetAll().ToList(); 
			//if (model == null) model = new List<AccTaskLib>();
			int contractID = (!string.IsNullOrWhiteSpace(Request.Params["ContractID"])) ? int.Parse(Request.Params["ContractID"]) : -1;
			var model = uow.Repository<Contract>().GetSingle(contractID)?.Tasks;

			//model.Insert(0, new AccTask() { Id = -1, Name = "[Tạo mới công việc]" });
			return PartialView(model);
		}
		public ActionResult GetTaskOfContractCreateFromLib()
		{
			//var model = uow.Repository<AccTaskLib>().GetAll().ToList(); 
			//if (model == null) model = new List<AccTaskLib>();
			int contractID = (!string.IsNullOrWhiteSpace(Request.Params["ContractID"])) ? int.Parse(Request.Params["ContractID"]) : -1;
			var model = uow.Repository<Contract>().GetSingle(contractID)?.Tasks;

			//model.Insert(0, new AccTask() { Id = -1, Name = "[Tạo mới công việc]" });
			return PartialView(model);
		}
		public ActionResult GetTaskOfContractEdit()
		{
			int contractID = (!string.IsNullOrWhiteSpace(Request.Params["ContractID"])) ? int.Parse(Request.Params["ContractID"]) : -1;
			var AccTasks = uow.Repository<Contract>().GetSingle(contractID)?.Tasks ?? new List<AccTask>();

			//if (AccTasks.Count > 0) AccTasks.Insert(0, new AccTask() { Id = -1, Name = "[Tạo mới công việc]" });
			ViewData["AccTasks"] = AccTasks;
			var equipment = service.GetById(GridViewHelper.SelectedEquipmentID) ?? new Equipment();
			var model = equipment.contract.Tasks.Where(x => x.Accreditations.Select(e => e.equiment?.Id == equipment.Id).Count() > 0).FirstOrDefault();
			ViewData["AccTask"] = model;
			ViewData["IndexOfTask"] = AccTasks.IndexOf(model);
			return PartialView(model);
		}
		public ActionResult AccreditationView(int id)
		{
			//ViewData["Equipments"] = service.GetAll();
			GridViewHelper.SelectedEquipmentID = id;
			ViewData["EmployedStandards"] = uow.Repository<Standard>().GetAll();
			ViewData["EmployedProcedures"] = uow.Repository<Procedure>().GetAll();
			ViewData["AppUsers"] = uow.Repository<IncosafCMS.Core.DomainModels.Identity.AppUser>().GetAll();
			//Hưng sửa 6.5.2025 load DS KĐV vào combobox khi cột AppUser.TwoFactorEnabled = 1 (tạm update TwoFactor bằng tay)
			ViewData["AppUsersKDV2"] = uow.Repository<IncosafCMS.Core.DomainModels.Identity.AppUser>().FindBy(x => x.TwoFactorEnabled == true);

			if (GridViewHelper.SelectedEquipmentID < 0)
			{
				var model = new Accreditation();
				return View(model);
			}
			else
			{
				var model = uow.Repository<Accreditation>().FindBy(x => x.equiment != null && x.equiment.Id == GridViewHelper.SelectedEquipmentID).FirstOrDefault() ?? new Accreditation();
				return View(model);
			}
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult AccreditationView(int id, FormCollection collection)
		{
			try
			{
				if (string.IsNullOrEmpty(EditorExtension.GetValue<string>("Location")) ||
					string.IsNullOrEmpty(EditorExtension.GetValue<string>("Tester1Id")) ||
					string.IsNullOrEmpty(EditorExtension.GetValue<string>("CreateDate")) ||
					string.IsNullOrEmpty(EditorExtension.GetValue<string>("AccrDate")) ||
					string.IsNullOrEmpty(EditorExtension.GetValue<string>("DateOfNext")) ||
					string.IsNullOrEmpty(EditorExtension.GetValue<string>("AccrLocation")) ||
					string.IsNullOrEmpty(EditorExtension.GetValue<string>("StampNumber")) ||
					//string.IsNullOrEmpty(EditorExtension.GetValue<string>("TypeAcc")) ||
					string.IsNullOrEmpty(EditorExtension.GetValue<string>("AccrResultDate"))
					)
					return Json("required", JsonRequestBehavior.AllowGet);

				var accreditation = uow.Repository<Accreditation>().GetSingle(id);

				if (accreditation.AccTask.AccTaskNote == "KĐXD-TBN" || accreditation.AccTask.AccTaskNote == "KĐXD-TBAL")
				{
					if (string.IsNullOrEmpty(EditorExtension.GetValue<string>("TypeAcc")))
					{
						return Json("required", JsonRequestBehavior.AllowGet);
					}
				}

				accreditation.CreateDate = EditorExtension.GetValue<DateTime>("CreateDate");
				accreditation.AccrDate = EditorExtension.GetValue<DateTime>("AccrDate");
				accreditation.AccrResultDate = EditorExtension.GetValue<DateTime>("AccrResultDate");
				accreditation.DateOfNext = EditorExtension.GetValue<DateTime>("DateOfNext");

				if (User.IsInRole("Admin")) accreditation.NumberAcc = EditorExtension.GetValue<string>("txtNumberAcc");
				//accreditation.NumberSuggest = EditorExtension.GetValue<string>("NumberSuggest");
				accreditation.AccrLocation = EditorExtension.GetValue<string>("AccrLocation");
				accreditation.Viewer1 = EditorExtension.GetValue<string>("Viewer1");
				accreditation.PositionViewer1 = EditorExtension.GetValue<string>("PositionViewer1");
				accreditation.Viewer2 = EditorExtension.GetValue<string>("Viewer2");
				accreditation.PositionViewer2 = EditorExtension.GetValue<string>("PositionViewer2");

				accreditation.DocumentTechnicalNotice = EditorExtension.GetValue<string>("DocumentTechnicalNotice");
				accreditation.DocumentTechicalResult = EditorExtension.GetValue<string>("DocumentTechicalResult");
				accreditation.PartionsNotice = EditorExtension.GetValue<string>("PartionsNotice");
				accreditation.LoadTestNotice = EditorExtension.GetValue<string>("LoadTestNotice");
				accreditation.RequestsTime = EditorExtension.GetValue<string>("RequestsTime");


				//if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("equiment.Id_VI")))
				//{
				//    int equipmentID;
				//    if (int.TryParse(EditorExtension.GetValue<string>("equiment.Id_VI"), out equipmentID))
				//        accreditation.equiment = service.GetById(equipmentID);
				//}

				accreditation.Location = EditorExtension.GetValue<string>("Location");

				if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("EmployedStandard.Id")))
				{
					int employedStandardID;
					if (int.TryParse(EditorExtension.GetValue<string>("EmployedStandard.Id"), out employedStandardID))
						accreditation.EmployedStandard = uow.Repository<Standard>().GetSingle(employedStandardID);
				}

				if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("EmployedProcedure.Id")))
				{
					int employedProcedureID;
					if (int.TryParse(EditorExtension.GetValue<string>("EmployedProcedure.Id"), out employedProcedureID))
						accreditation.EmployedProcedure = uow.Repository<Procedure>().GetSingle(employedProcedureID);
				}

				//var lstTesterID = accreditation.Tester.Select(x => x.Id).ToList();

				var strTesters = EditorExtension.GetValue<string>("Testers_TKV").Replace("[\"", "").Replace("\"]", "").Replace("\"", "").Split(',');

				List<int> lstTesterIDModified = new List<int>();
				foreach (var testerID in strTesters)
				{
					int idTester = -1;
					if (int.TryParse(testerID, out idTester) && idTester > 0)
					{
						lstTesterIDModified.Add(idTester);
					}
				}

				//var lstExceptID = lstTesterIDModified.Except(lstTesterID).Union(lstTesterID.Except(lstTesterIDModified));
				//foreach (var item in lstExceptID)
				//{
				//    if (lstTesterID.Contains(item))
				//    {
				//        var tester = accreditation.Tester.Where(x => x.Id == item).FirstOrDefault();
				//        if (tester != null)
				//            accreditation.Tester.Remove(tester);
				//    }
				//    else
				//    {
				//        var tester = uow.Repository<IncosafCMS.Core.DomainModels.Identity.AppUser>().GetSingle(item);
				//        if (tester != null)
				//            accreditation.Tester.Add(tester);
				//    }
				//}

				if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("TypeAcc")) || EditorExtension.GetValue<string>("TypeAcc") == "ChuaChon")
				{
					int typeAccID;
					if (int.TryParse(EditorExtension.GetValue<string>("TypeAcc"), out typeAccID))
						accreditation.TypeAcc = (TypeOfAccr)typeAccID;
				}

				//if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("CorrespondingLoad")))
				//{
				//    double correspondingLoad;
				//    if (double.TryParse(EditorExtension.GetValue<string>("CorrespondingLoad"), out correspondingLoad))
				//        accreditation.CorrespondingLoad = correspondingLoad;
				//}

				//if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("StaticLoad")))
				//{
				//    double staticLoad;
				//    if (double.TryParse(EditorExtension.GetValue<string>("StaticLoad"), out staticLoad))
				//        accreditation.StaticLoad = staticLoad;
				//}

				//if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("DynamicLoad")))
				//{
				//    double dynamicLoad;
				//    if (double.TryParse(EditorExtension.GetValue<string>("DynamicLoad"), out dynamicLoad))
				//        accreditation.DynamicLoad = dynamicLoad;
				//}
				
				accreditation.StampNumber = EditorExtension.GetValue<string>("StampNumber");
				accreditation.StampLocated = EditorExtension.GetValue<string>("StampLocated");
				accreditation.Requests = EditorExtension.GetValue<string>("Requests");

				var strIsCompleted = EditorExtension.GetValue<string>("IsCompleted");
				bool isCompleted = strIsCompleted == "True";
				accreditation.IsCompleted = isCompleted;

				var strAccreditationResult = EditorExtension.GetValue<string>("AccreditationResult");
				bool accreditationResult = strAccreditationResult == "True";
				accreditation.AccreditationResult = accreditationResult;

				var existed = uow.Repository<Accreditation>().GetSingle(e => e.StampNumber == accreditation.StampNumber);                
				if (existed != null && existed.Id != accreditation.Id)                    
				{                    
					return Json("stamp_number_existed", JsonRequestBehavior.AllowGet);
				}

				uow.Repository<Accreditation>().Update(accreditation);
				uow.SaveChanges();
				return Json("success", JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
				return Json("error", JsonRequestBehavior.AllowGet);
			}

			//return RedirectToAction("Equipment", "Home");
		}

		/// <summary>
		/// Noted by lapbt 
		/// 08-jun-2023. Edit bien ban kiem dinh
		/// </summary>
		/// <param name="id"></param>
		/// <param name="contractId"></param>
		/// <param name="numEquips"></param>
		/// <returns></returns>
		public ActionResult AccreditationViewViaContract(int id, string contractId, int numEquips=1)
		{
			//ViewData["Equipments"] = service.GetAll();
			if(int.TryParse(contractId, out int int_contractId))
			{
				GridViewHelper.SelectedEquipmentID = id;
				//9.9.2025 bỏ lấy Standard; Procedure do không dùng đến
				//ViewData["EmployedStandards"] = uow.Repository<Standard>().GetAll();
				//ViewData["EmployedProcedures"] = uow.Repository<Procedure>().GetAll();
				ViewData["AppUsers"] = uow.Repository<AppUser>().GetAll();
				//Hưng sửa 6.5.2025 load DS KĐV vào combobox khi cột AppUser.TwoFactorEnabled = 1 (tạm update TwoFactor bằng tay)
				ViewData["AppUsersKDV2"] = uow.Repository<AppUser>().FindBy(x => x.TwoFactorEnabled == true);
				var provinces = uow.Repository<Province>().GetAll();
				ViewData["provinces"] = provinces;
				ViewData["numEquips"] = numEquips;//sửa từ ViewData["NumEquips"] = numEquips;

				var contract = uow.Repository<Contract>().GetSingle(e => e.Id == int_contractId);
				ViewData["Contract"] = contract;
				var tasks = contract.Tasks;

				if (GridViewHelper.SelectedEquipmentID < 0)
				{
					var model = new Accreditation();
					return View(model);
				}
				else
				{
					var model = uow.Repository<Accreditation>().FindBy(x => x.equiment != null && x.equiment.Id == GridViewHelper.SelectedEquipmentID).FirstOrDefault() ?? new Accreditation();

					if (string.IsNullOrEmpty(model.AccrLocation))
					{
						// 18-mar-2024. edited by lapbt. ktra nếu địa điểm bên hợp đồng là 1 tỉnh/thành cụ thể thì dùng, còn k0 để trống
						var prov = uow.Repository<Province>().FindBy(e => e.ProvinceName == contract.DiaDiemThucHien).FirstOrDefault();
						if (prov != null && !string.IsNullOrEmpty(prov.ProvinceName))
						{
							model.AccrLocation = contract.DiaDiemThucHien;//?.ProvinceName;
							model.Ma_TP = prov.Ma_TP;
						} else
						{
							model.AccrLocation = "";
							model.Ma_TP = " ";
						}
					} 
					else if (!string.IsNullOrEmpty(model.AccrLocation) && string.IsNullOrEmpty(model.Ma_TP))
					{
						// 01-apr-2024. Nếu là dạng DL chưa đầy đủ, ở Ma_TP thì cũng bổ sung
						var prov = uow.Repository<Province>().FindBy(e => e.ProvinceName == model.AccrLocation).FirstOrDefault();
						if (!string.IsNullOrEmpty(prov.ProvinceName))
						{
							model.Ma_TP = prov.Ma_TP;
						}
					}

					// 01-apr-2024. Load DS QH theo TP, nếu trc đó đã có lựa chọn
					/* Tạm đóng chỗ này do thay đổi QH, PX từ T7.2025
					if (!string.IsNullOrEmpty(model.Ma_TP))
					{
						ViewData["ProvincesFull_QH"] = uow.Repository<Core.DomainModels.v_ProvinceDistrict>().FindBy(x => x.Ma_TP == model.Ma_TP).ToList();

						// và DS PX
						if (!string.IsNullOrEmpty(model.Ma_QH))
						{
							ViewData["ProvincesFull_QH_PX"] = uow.Repository<Core.DomainModels.ProvinceFull>().FindBy(x => x.Ma_TP == model.Ma_TP && x.Ma_QH == model.Ma_QH).ToList();
						}
					}
					*/
					//21.05.2025 Load TSKT	
					//Nếu là KĐAT thì load TSKT					
					if (model.AccTask.AccTaskNote == "KĐXD-TBN" || model.AccTask.AccTaskNote == "KĐXD-TBAL")
					{
						var spec = uow.Repository<Equipment>().GetSingle(e => e.Id == model.equipmentId);
						if (model.equiment.specifications.Count != 0)
						{
							//Khi Edit TB đã có thì load TSKT từ TB cũ
							EquipmentSpecificationsList.GetEquipmentSpecifications = spec.specifications ?? new List<Specifications>();
						}
						else
						//Khi Create TB mới hoặc TB chưa có TSKT thì load TSKT từ TB mẫu
						//07.06.2025 sửa: Chưa làm được!
						//Cần xử lý load: từ Mã TB lấy OriginalEquipment_Id từ bảng OriginalEquipment
						//Load các TSKT có OriginalEquipment_Id tương ứng ở bảng specifications
						{
							//var specCreate = uow.Repository<Equipment>().GetSingle(e => e.mahieu == model.equipmentCode.ToString() + ".0000");
							//spec = uow.Repository<Equipment>().GetSingle(e => e.Id == specCreate.Id);
							/*
							spec = uow.Repository<Equipment>().GetSingle(e => e.mahieu == model.equipmentCode.ToString() + ".0000");
							EquipmentSpecificationsList.GetEquipmentSpecifications = spec.specifications ?? new List<Specifications>();
							foreach (var specification in spec.specifications)
							{
								var newSpecification = new Specifications()
								{
									Id = 0,
									Name = specification.Name,
									Value = specification.Value,
									f_key = specification.f_key,
									f_unit = specification.f_unit
								};
								model.equiment.specifications.Add(newSpecification);
								uow.Repository<Specifications>().Insert(newSpecification);								
							}
							*/

							// Khi Create TB mới hoặc TB chưa có TSKT thì load từ OriginalSpec							
							var originalSpecs = uow.Repository<OriginalSpec>()
													.FindBy(os => os.OriginalEquipment_Code == spec.Code)
													.ToList();

							foreach (var originalSpec in originalSpecs)
							{
								var newSpecification = new Specifications()
								{
									Id = 0,
									Name = originalSpec.Name,
									Value = "", // luôn để rỗng khi tạo mới
									Equipment_Id = spec.Id,
									f_key = originalSpec.f_key,
									f_unit = originalSpec.f_unit
								};

								model.equiment.specifications.Add(newSpecification);
								uow.Repository<Specifications>().Insert(newSpecification);
							}

							uow.SaveChanges();
							//Trả lại EquipmentSpecificationsList theo equipmentId hiện tại
							spec = uow.Repository<Equipment>().GetSingle(e => e.Id == model.equipmentId);
							EquipmentSpecificationsList.GetEquipmentSpecifications = spec.specifications ?? new List<Specifications>();
						}
					}
					else EquipmentSpecificationsList.GetEquipmentSpecifications = null;



					// 13/02/2023. added by lapbt, mac dinh de ten KH, dia chi KH
					/* tam bo, vi them customer_id vao se bi loi tao hop dong
					var cust_of_contract = uow.Repository<Customer>().FindBy(x => x.Id == contract.customer_id).First();
					if (cust_of_contract != null)
					{
						if (string.IsNullOrEmpty(model.PartionsNotice))
							model.PartionsNotice = cust_of_contract.Name;
						if (string.IsNullOrEmpty(model.LoadTestNotice))
							model.LoadTestNotice = cust_of_contract.Address;
					}
					*/

					//18.06.2025 Hưng mở để load ĐVSD khi đã khai báo hợp đồng vào TB
					if (contract.Proprietor != null)
                    {
						var cust_of_contract = uow.Repository<Proprietor>().FindBy(x => x.Id == contract.Proprietor.Id).First();
						if (cust_of_contract != null)
						{
							if (string.IsNullOrEmpty(model.PartionsNotice))
							{
								model.PartionsNotice = cust_of_contract.PropName;
								ViewData["PartionsNotice"] = model.PartionsNotice;

							}

							if (string.IsNullOrEmpty(model.LoadTestNotice))
							{
								model.LoadTestNotice = cust_of_contract.PropAddress;
								ViewData["PartionsNotice"] = model.LoadTestNotice;
							}

						}
					}
					
					//Khi khởi tạo TB, mặc định KDV1 là chủ trì, có thể chọn thay đổi ở combobox 
					if (model.Tester1Id == null)
					{ 
						model.Tester1Id = contract.own.Id;
					}

					return View(model);
				}
			}
			else
			{
				var model = new Accreditation();
				return View(model);
			}
			
		}

		/// <summary>
		/// Hàm này để trả về loại tem, có định nghĩa ở bảng StampType, nhg đang hardcode ở đây
		/// </summary>
		/// <param name="AccTaskNote"></param>
		/// <returns></returns>
		private int __getStampTypeId(string AccTaskNote)
		{
			int stampTypeId = 0;
			switch (AccTaskNote) //accreditation.AccTask.AccTaskNote
			{
				case "KĐXD-TBN":
					stampTypeId = 1;
					break;
				case "KĐXD-TBAL":
					stampTypeId = 1;
					break;
				case "KĐXD-AK":
					stampTypeId = 2;
					break;
				case "KĐXD-HQ":
					stampTypeId = 3;
					break;
			}
			return stampTypeId;
		}

		/// <summary>
		/// Căn cứ vào 2 yếu tố: Loại tem & Số tem thực dán (7 số)
		/// Để sinh ra số accreditation.AccTask.StampSerial là kiểu số, dùng để so sánh với phần qly phân bổ tem
		/// </summary>
		/// <param name="AccTaskNote">accreditation.AccTask.AccTaskNote</param>
		/// <param name="StampNumber">accreditation.AccTask.StampNumber</param>
		/// <returns></returns>
		private int __getStampSerial(string AccTaskNote, int StampNumber)
		{
			// do số tem thực có độ dài 7 ký số, đưa về chuỗi để chèn thêm loại tem vào
			string sstampnum = StampNumber.ToString();
			// stampserial format: YY#NNNNN
			string stampserial = sstampnum.Substring(0,2) + __getStampTypeId(AccTaskNote).ToString() + sstampnum.Substring(sstampnum.Length-5);

			int stampSerial;
			int.TryParse(stampserial, out stampSerial);
			return stampSerial;
		}


		/*
		 * Note by Lapbt
		 * Sau khi gọi hàm __getNewTestingResultNumber để lấy số BBKD xong sẽ tới đây để hiển thị
		 * Event: sự kiện này đc thực hiện khi với 2 nút: Nút "Lưu lại" để Save BBKD && Nút "Lấy số" cg sẽ save lại vào DB luôn (~ nhấn nút Lưu ở dưới)
		 */
		[HttpPost, ValidateInput(false)]
		public ActionResult AccreditationViewViaContract(int id, int accTask_Id, FormCollection collection, bool skipTSKT = false)		
		{
			try
			{
				int serialnumb = 0;
				
				//Tạo thư mục EditorExtension.GetValue để dùng nhiều lần
				var formValues = new Dictionary<string, string>
				{
					{ "AccrDate", EditorExtension.GetValue<string>("AccrDate") },
					{ "AccrResultDate", EditorExtension.GetValue<string>("AccrResultDate") },
					{ "AccreditationResult", EditorExtension.GetValue<string>("AccreditationResult") },
					{ "AccrLocation", EditorExtension.GetValue<string>("AccrLocation") },
					{ "CreateDate", EditorExtension.GetValue<string>("CreateDate") },
					{ "cmbProvinces", EditorExtension.GetValue<string>("cmbProvinces") },
					{ "cmbProvinces_QH", EditorExtension.GetValue<string>("cmbProvinces_QH") },
					{ "cmbProvinces_QH_PX", EditorExtension.GetValue<string>("cmbProvinces_QH_PX") },
					{ "DateOfNext", EditorExtension.GetValue<string>("DateOfNext") },
					{ "DocumentTechnicalNotice", EditorExtension.GetValue<string>("DocumentTechnicalNotice") },
					{ "DocumentTechicalResult", EditorExtension.GetValue<string>("DocumentTechicalResult") },
					{ "equiment.No", EditorExtension.GetValue<string>("equiment.No") },
					{ "equiment.Name", EditorExtension.GetValue<string>("equiment.Name") },
					{ "equiment.ManuFacturer", EditorExtension.GetValue<string>("equiment.ManuFacturer") },
					{ "equiment.YearOfProduction", EditorExtension.GetValue<string>("equiment.YearOfProduction") },
					{ "equiment.mahieu", EditorExtension.GetValue<string>("equiment.mahieu") },
					{ "IsCompleted", EditorExtension.GetValue<string>("IsCompleted") },
					{ "Location", EditorExtension.GetValue<string>("Location") },
					{ "LoadTestNotice", EditorExtension.GetValue<string>("LoadTestNotice") },
					{ "MissingDocs", EditorExtension.GetValue<string>("MissingDocs") },
					{ "NumberAcc", EditorExtension.GetValue<string>("NumberAcc") },
					{ "PartionsNotice", EditorExtension.GetValue<string>("PartionsNotice") },
					{ "PositionViewer1", EditorExtension.GetValue<string>("PositionViewer1") },
					{ "PositionViewer2", EditorExtension.GetValue<string>("PositionViewer2") },
					{ "Requests", EditorExtension.GetValue<string>("Requests") },
					{ "RequestsTime", EditorExtension.GetValue<string>("RequestsTime") },
					{ "StampNumber", EditorExtension.GetValue<string>("StampNumber") },
					{ "StampLocated", EditorExtension.GetValue<string>("StampLocated") },					
					{ "Tester1Id", EditorExtension.GetValue<string>("Tester1Id") },
					{ "Tester2Id", EditorExtension.GetValue<string>("Tester2Id") },
					{ "TypeAcc", EditorExtension.GetValue<string>("TypeAcc") },
					{ "Viewer1", EditorExtension.GetValue<string>("Viewer1") },
					{ "Viewer2", EditorExtension.GetValue<string>("Viewer2") },
					// ... bổ sung những field còn lại
				};

				if (
					string.IsNullOrEmpty(formValues["Tester1Id"]) ||
					string.IsNullOrEmpty(formValues["CreateDate"]) ||
					string.IsNullOrEmpty(formValues["AccrDate"]) ||
					string.IsNullOrEmpty(formValues["NumberAcc"]) ||
					string.IsNullOrEmpty(formValues["DateOfNext"]) ||
					string.IsNullOrEmpty(formValues["cmbProvinces"]) ||
					string.IsNullOrEmpty(formValues["AccrResultDate"])
					)
					return Json("required", JsonRequestBehavior.AllowGet);

				// 03-jan-2024 by lapbt. Kiểm tra tên có chữ khác thì báo lỗi				
				var chkEquipname = formValues["equiment.Name"];
				if (string.IsNullOrEmpty(chkEquipname) || chkEquipname.Contains("khác") || chkEquipname.Contains("Khác") || chkEquipname.Contains("(khác)") || chkEquipname.Contains("(Khác)"))
				{
					return Json("invalid_equip_name", JsonRequestBehavior.AllowGet);
				}								
				
				//TODO: Lấy ra tất cả các kết quả kiểm định thuộc cùng một công việc và đánh số kết quả kđ
				var accreditation = uow.Repository<Accreditation>().GetSingle(id);				
				var contract = uow.Repository<Contract>().GetSingle(e => e.Id == accreditation.equiment.contract.Id);
				bool isTBN = accreditation.AccTask.AccTaskNote == "KĐXD-TBN";
				bool isTBAL = accreditation.AccTask.AccTaskNote == "KĐXD-TBAL";
				bool isAK = accreditation.AccTask.AccTaskNote == "KĐXD-AK";
				bool isHQ = accreditation.AccTask.AccTaskNote == "KĐXD-HQ";

				//Cho phép sửa BBKĐ trong vòng 60 ngày từ ngày khởi tạo BB và HĐ đã cấp số
				TimeSpan ktraCreateDate = (TimeSpan)(DateTime.Today - accreditation.CreateDate);					
				if (!User.IsInRole("Admin") && !string.IsNullOrEmpty(contract.MaHD) && ktraCreateDate.TotalDays > 60)
                {
					return Json("hansuaBBKD", JsonRequestBehavior.AllowGet);
				}

				// 13/02/2023 by lapbt. Bat buoc neu la thiet bi thuoc BXD				 
				if (string.IsNullOrEmpty(formValues["Location"]) ||
					string.IsNullOrEmpty(formValues["equiment.No"]) )
				{
					var originalEquipment = uow.Repository<OriginalEquipment>().GetSingle(e => e.Code == accreditation.equiment.Code && e.Name == accreditation.equiment.Name);
					if (originalEquipment != null)
					{
						if (originalEquipment.AccreditorType == "BXD")
						{
							return Json("required", JsonRequestBehavior.AllowGet);
						}
					}
				}

				//Hưng thêm 2.5.2025: Ktra chứng chỉ KDV1 đối với thiết bị đang lấy số KQKĐ đối chiếu qua bảng Procedure
				if (isTBN || isTBAL || isAK || isHQ)
				{
					var tester1 = formValues["Tester1Id"] ?? "";
					var tester2 = formValues["Tester2Id"] ?? "";                
					var cerKDV1 = uow.Repository<Procedure>().FindBy(e => e.Description == accreditation.equipmentCode && e.Name == tester1).Count;
					var cerKDV2 = uow.Repository<Procedure>().FindBy(e => e.Description == accreditation.equipmentCode && e.Name == tester2).Count;
					if (cerKDV1 + cerKDV2 == 0)
					{                       
					return Json("Cert_KDV1", JsonRequestBehavior.AllowGet);                        
					}
				}

				// Hưng thêm 6.3.2025 ktra hạn KĐ > ngày KĐ; Ngày ký <= 10 ngày KĐ
				TimeSpan chkHankd = EditorExtension.GetValue<DateTime>("AccrDate") - EditorExtension.GetValue<DateTime>("DateOfNext");
				if (chkHankd.TotalDays >= 0 && (isTBN || isTBAL))
				{
					return Json("hankd", JsonRequestBehavior.AllowGet);
				}
				TimeSpan chkNgayky = EditorExtension.GetValue<DateTime>("AccrResultDate") - EditorExtension.GetValue<DateTime>("AccrDate");
				if (chkNgayky.TotalDays > 10 || chkNgayky.TotalDays < 0)
				{
					return Json("Ngayky", JsonRequestBehavior.AllowGet);
				}

				/* 28-Oct-2021. Edited by lapbt
				 * Để trường số tem KĐ là dạng số, và là trường bắt buộc phải nhập khi loại hình công việc là: KĐXD-TBN hoặc KĐXD-TBAL
				 * 10.04.2025 Hưng thêm áp kế là bắt buộc nhập số tem và hình thức kiểm định 
				 * Các loại hình khác thì là không bắt buộc.
				 */
				if (isTBN || isTBAL || isAK)
				{
					//Kiểm tra ô số tem không được để trống
					if (string.IsNullOrEmpty(formValues["StampNumber"]))
					{
						return Json("Nhap_sotem", JsonRequestBehavior.AllowGet);
					}
					//Kiểm tra ô Hình thức KĐ không được để trống
					if (string.IsNullOrEmpty(formValues["TypeAcc"]) || formValues["TypeAcc"] == "ChuaChon")
					{
						return Json("Nhap_HTKD", JsonRequestBehavior.AllowGet);
					}					                 
				}

				var numberAcc = formValues["NumberAcc"];
				if(!string.IsNullOrWhiteSpace(accreditation.NumberAcc) && !string.IsNullOrWhiteSpace(numberAcc))
				{
					if(accreditation.NumberAcc != numberAcc)
					{
						 return Json("numberAccAsigned", JsonRequestBehavior.AllowGet);
					}
				}

				var accredits = accreditation.AccTask.Accreditations;				
				accreditation.CreateDate = EditorExtension.GetValue<DateTime>("CreateDate");
				accreditation.AccrDate = EditorExtension.GetValue<DateTime>("AccrDate");
				accreditation.AccrResultDate = EditorExtension.GetValue<DateTime>("AccrResultDate");
				accreditation.DateOfNext = EditorExtension.GetValue<DateTime>("DateOfNext");				
				accreditation.NumberAcc = formValues["NumberAcc"];				
				accreditation.AccrLocation = formValues["AccrLocation"];
				accreditation.Location = formValues["Location"];
				accreditation.Viewer1 = formValues["Viewer1"];
				accreditation.PositionViewer1 = formValues["PositionViewer1"];
				accreditation.Viewer2 = formValues["Viewer2"];
				accreditation.PositionViewer2 = formValues["PositionViewer2"];

				//Đóng DocumentTechnicalNotice; DocumentTechicalResult; RequestsTime; MissingDocs do không sử dụng
				accreditation.DocumentTechnicalNotice = formValues["DocumentTechnicalNotice"];
				accreditation.DocumentTechicalResult = formValues["DocumentTechicalResult"];
				accreditation.RequestsTime = formValues["RequestsTime"];
				accreditation.MissingDocs = formValues["MissingDocs"];

				accreditation.PartionsNotice = formValues["PartionsNotice"];
				accreditation.LoadTestNotice = formValues["LoadTestNotice"];				
				// 13/02/2023 added by lapbt, get So che tao
				accreditation.equiment.No = formValues["equiment.No"];
				// 20/05/2025 Hung them: Nhà SX; Năm SX; Mã hiệu
				accreditation.equiment.ManuFacturer = formValues["equiment.ManuFacturer"];
				accreditation.equiment.YearOfProduction = formValues["equiment.YearOfProduction"];
				accreditation.equiment.mahieu = formValues["equiment.mahieu"];
				// 03/01/2024 added by lapbt, update lại tên thiết bị, với trường hợp có nhập vào khi là thiết bị khác. Còn lại tên chuẩn đã đặt readonly
				accreditation.equiment.Name = formValues["equiment.Name"];
				// 01-apr-2024. Edit by Lapbt. Add detail Ma TP/QH/PX
				var provinceIdstr = formValues["cmbProvinces"];
				var provinceQH_Idstr = formValues["cmbProvinces_QH"];
				var provincePX_Idstr = formValues["cmbProvinces_QH_PX"];
				var s = int.TryParse(provinceIdstr, out int provinceId);

				if (!string.IsNullOrEmpty(provinceIdstr) && s)
				{
					// var province = uow.Repository<Province>().GetSingle(provinceId);
					var province = uow.Repository<Province>().FindBy(x => x.Ma_TP == provinceIdstr).First();
					if (province != null)
					{
						accreditation.AccrLocation = province.ProvinceName;
						accreditation.Ma_TP = provinceIdstr;
						accreditation.Ma_QH = provinceQH_Idstr;
						accreditation.Ma_PX = provincePX_Idstr;
					}
				}
				else
				{
					accreditation.AccrLocation = provinceIdstr;
					accreditation.Ma_TP = "";
					accreditation.Ma_QH = "";
					accreditation.Ma_PX = "";
				}
				//11.9.2025 Đóng trường EmployedProcedure.Id và EmployedProcedure.Id do không sử dụng đến
				
				if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("EmployedStandard.Id")))
				{
					int employedStandardID;
					if (int.TryParse(EditorExtension.GetValue<string>("EmployedStandard.Id"), out employedStandardID))
						accreditation.EmployedStandard = uow.Repository<Standard>().GetSingle(employedStandardID);
				}

				if (!string.IsNullOrEmpty(EditorExtension.GetValue<string>("EmployedProcedure.Id")))
				{
					int employedProcedureID;
					if (int.TryParse(EditorExtension.GetValue<string>("EmployedProcedure.Id"), out employedProcedureID))
						accreditation.EmployedProcedure = uow.Repository<Procedure>().GetSingle(employedProcedureID);
				}
				
				if (!string.IsNullOrEmpty(formValues["Tester1Id"]))
				{
					int t1id = 0;
					if (int.TryParse(formValues["Tester1Id"], out t1id))
						accreditation.Tester1Id = t1id;
				}
				if (!string.IsNullOrEmpty(formValues["Tester2Id"]))
				{
					int t2id = 0;
					if (int.TryParse(formValues["Tester2Id"], out t2id))
						accreditation.Tester2Id = t2id;
				}
				if (!string.IsNullOrEmpty(formValues["TypeAcc"]))
				{
					var sel = EditorExtension.GetValue<TypeOfAccr>("TypeAcc");					
					accreditation.TypeAcc = sel;
				}

				accreditation.StampLocated = formValues["StampLocated"];
				accreditation.Requests = formValues["Requests"];//đóng do không sử dụng

				var strIsCompleted = formValues["IsCompleted"];
				bool isCompleted = strIsCompleted == "True";
				accreditation.IsCompleted = isCompleted;

				var strAccreditationResult = formValues["AccreditationResult"];
				bool accreditationResult = strAccreditationResult == "True";
				accreditation.AccreditationResult = accreditationResult;

				/*
				 * Khúc dưới này sẽ xly hàng loạt cho các TB tương tự
				 * Note: nếu có nhập vào số tem, sẽ kiểm tra xem có bao nhiêu tbi sẽ lấy số lần này để gen ra số tem cho từng đó tbi luôn
				 */

				if (!string.IsNullOrEmpty(formValues["StampNumber"]))
				{
					// 0. Thông tin chung về tem                    
					var inpStampNumber = formValues["StampNumber"].Trim();        // số tem nhập vào dạng chuỗi
					int.TryParse(inpStampNumber, out int stampnumb);                                    // số tem nhập vào dạng số
					var stampno_format = System.Configuration.ConfigurationManager.AppSettings["stampNoFormat"];    

					if (stampno_format == "number" && stampnumb <= 0)
					{
						return Json("stamp_number_only", JsonRequestBehavior.AllowGet);
					}

					// 1. xử lý Serial cho tem, khi thuộc loại tem KĐAT, AK, HQ
					if (stampnumb > 0 && __getStampTypeId(accreditation.AccTask.AccTaskNote) > 0)
					{
						// kiểm tra độ fai 7 ký số
						if (inpStampNumber.Length != 7)
						{
							return Json("invaild_stamp_number_length", JsonRequestBehavior.AllowGet);
						}

						// sinh số Serial tbi hiện tại
						serialnumb = __getStampSerial(accreditation.AccTask.AccTaskNote, stampnumb);

						// 1. ktra trung số Serial. Trả về tên KDV, Số HĐ, Tên TB đã dùng tem đó (nếu tìm thấy theo testerid1)
						var existed = uow.Repository<Accreditation>().GetSingle(e => e.Id != accreditation.Id && e.SerialNumber == serialnumb && e.NumberAcc != null);
						if (existed != null && existed.Id != accreditation.Id)
						{
							string retKey = "stamp_number_existed";
							var tester = uow.Repository<AppUser>().GetSingle(e => e.Id == existed.Tester1Id);
							var chkContractStamp = uow.Repository<Equipment>().GetSingle(e => e.Id == existed.equiment.Id);
							if (tester != null) retKey += "#" + tester.DisplayName + " - Tại hợp đồng số: " + chkContractStamp.contract.MaHD + " - Tên TB: " + chkContractStamp.Name;// + "; Ngày KĐ: " + difference.TotalDays;
							return Json(retKey, JsonRequestBehavior.AllowGet);
						}

						// 2. ktra phạm vi tem được giao (bao gồm cả 2 KĐV), không tính tem đã thu hồi/hủy						
						var temKDV = uow.Repository<StampSerial>().GetSingle(e => e.SerialNumber == serialnumb && e.Status == 0);
						
						// 2.1 Tem chưa được giao
						if (temKDV == null)
                        {
							//Lấy Id đơn vị của Chủ trì/KDV1/KDV2							
							var user = userManager.FindByName(User.Identity.Name);							
							var departOwner = user.Department.Id;							
							var departkdv1 = uow.Repository<AppUser>().GetSingle(e => e.Id == accreditation.Tester1Id).DepartmentId;
														
							if (!string.IsNullOrEmpty(formValues["Tester2Id"]))
                            {
								var departkdv2 = uow.Repository<AppUser>().GetSingle(e => e.Id == accreditation.Tester2Id).DepartmentId;
								//Chủ trì tại HN
								if (departOwner < 7 && ((departkdv1 != null && departkdv1 > 7) || (departkdv2 != null && departkdv2 > 7)))
								{
									return Json("KDV_taiCN", JsonRequestBehavior.AllowGet);
								}
								//Chủ trì tại Chi nhánh
								if (departOwner == 8 || departOwner == 10)
								{
									if ((departkdv1 != null && departkdv1 != departOwner) || (departkdv2 != null && departkdv2 != departOwner))
										return Json("KDV_khacVung", JsonRequestBehavior.AllowGet);
								}
							}
							
							//Nếu 2 kđv cùng vùng miền với chủ trì thì thông báo tem chưa được giao
							string retKey2 = "invaild_stamp_not";							
							retKey2 += "#" + "Số tem: " + serialnumb.ToString().Substring(0, 2) + serialnumb.ToString().Substring(3, 5) + " chưa được giao cho KĐV nào.";
							return Json(retKey2, JsonRequestBehavior.AllowGet);
						}							
						// 2.2 Tem đã giao cho KĐV khác
						if (temKDV != null && temKDV.OwnerId != accreditation.Tester1Id && temKDV.OwnerId != accreditation.Tester2Id)
                        {
							string retKey3 = "stamp_number_out";
							var testerStamp = uow.Repository<AppUser>().GetSingle(e => e.Id == temKDV.OwnerId);
							if (testerStamp != null) retKey3 += "#" + serialnumb.ToString().Substring(0,2) + serialnumb.ToString().Substring(3, 5) + " đã giao cho KĐV " + testerStamp.DisplayName;
							return Json(retKey3, JsonRequestBehavior.AllowGet);
							//return Json("invaild_stamp_out", JsonRequestBehavior.AllowGet);
						}
						
						// 3 Ghi lại số serial hiện tại
						accreditation.SerialNumber = serialnumb;
						
					} else {
						// khi k0 fai loại tb KĐAT, AK, HQ thì số serial = 0
						accreditation.SerialNumber = 0;
					}

					// 5. nhận số tem cho tbi hiện tại, sau khi đã ktra. Cho bất cứ loại TB nào
					accreditation.StampNumber = inpStampNumber;

				}

				//Kiểm tra có TSKT chưa, nếu chưa có thì hỏi có nhập luôn không, nếu không muốn nhập thì chạy tiếp
				bool isSpec = false;
				if (!skipTSKT && (isTBN || isTBAL))
				{
					var checkspec = uow.Repository<Equipment>().GetSingle(e => e.Id == accreditation.equipmentId);

					foreach (var incomingSpec in checkspec.specifications)
					{
						if (!string.IsNullOrEmpty(incomingSpec.Value))
							isSpec = true;
					}

					if (!isSpec)
					{
						//Tạm đóng 8.9.2025 do khi show thông báo hỏi "Nhập TSKT" sẽ mất popup "Đang cập nhật thông số TB"
						//return Json("Nhap_TSKT", JsonRequestBehavior.AllowGet);
					}
				}
				
				// ghi lại toàn bộ cho BBKD hiện tại. Sẽ để sau so với ghi các tbi kèm theo. Có thể để nếu lỗi thì k0 cập nhật cho tbi hiện tại.
				// K0 move code lên trên, do ở trong khối xly tem vẫn cập nhật cho tb hiện tại
				uow.Repository<Accreditation>().Update(accreditation);				
				uow.SaveChanges();

				try
				{
					var numAcc = EditorExtension.GetValue<int>("SoThietBi");
					if (numAcc > 1)
					{
						var equipments = EquipmentAddedList.EquipmentsAdded;
						var contractId = accreditation.equiment.contract.Id;
						// Ngày kiểm định (bắt buộc đã có)
						var accrDate = accreditation.AccrDate;
						// Lấy list số kết quả KĐ cho các thiết bị còn lại
						var sokqkds = __getNewTestingResultNumber(
							contractId,
							accreditation.AccTask_Id.GetValueOrDefault(),
							numAcc - 1, accrDate.Value);

						// ==== 1. Chuẩn bị dữ liệu cần thiết trước vòng lặp ====
						//Tạo danh sách tem cho số TB sẽ kiểm tra
						var serialNumbersToCheck = Enumerable.Range(serialnumb + 1, numAcc).ToList();

						// Accreditation trùng số tem
						var existedAccreditations = uow.Repository<Accreditation>()
							.FindBy(e => e.Id != accreditation.Id
									&& e.NumberAcc != null
									&& e.SerialNumber.HasValue
									&& serialNumbersToCheck.Contains(e.SerialNumber.Value))
							.ToList();

						// StampSerial cho list số tem
						var existedStamps = uow.Repository<StampSerial>()
							.FindBy(e => serialNumbersToCheck.Contains(e.SerialNumber) && e.Status == 0)
							.ToList();

						// Dictionary thiết bị
						var equipmentIds = equipments.Select(eq => eq.Id).ToList();
						var equipmentDict = uow.Repository<Equipment>()
							.FindBy(e => equipmentIds.Contains(e.Id))
							.ToDictionary(e => e.Id, e => e);

						// Dictionary người kiểm định
						// Nếu OwnerId là int (non-nullable), ép về int? rồi lại lấy .Value sau khi lọc HasValue
						var userIds = existedAccreditations
							.Select(x => x.Tester1Id).Where(id => id.HasValue).Select(id => id.Value).AsEnumerable()
							.Concat(
								existedAccreditations.Select(x => x.Tester2Id)
									.Where(id => id.HasValue)
									.Select(id => id.Value)
									.AsEnumerable()
							)
							.Concat(
								existedStamps.Select(x => (int?)x.OwnerId) // ép về int? để đồng kiểu
									.Where(id => id.HasValue)
									.Select(id => id.Value)
									.AsEnumerable()
							)
							.Distinct()
							.ToList();

						var userDict = uow.Repository<AppUser>()
							.FindBy(u => userIds.Contains(u.Id))
							.ToDictionary(u => u.Id, u => u);

						// ==== 2. Vòng lặp xử lý ====
						var sokqInx = 0;
						//Lấy danh sách 1 lần từ Accreditation có equipmentId mới tạo bước trước
						//Làm như này để ở vòng lặp for không phải gọi từng cái ở db
						/*
						var accDict = uow.Repository<Accreditation>()
						.FindBy(x => equipmentIds.Contains(x.equipmentId))
						.ToDictionary(x => x.equipmentId, x => x);
						*/
						for (int i = 1; i < numAcc; i++)
						{
							var eq = equipments[i];
							var acc = uow.Repository<Accreditation>().FindBy(x => x.equiment != null && x.equiment.Id == eq.Id).FirstOrDefault();
							//var acc = accDict[eq.Id];
							if (serialnumb > 0)
							{
								int serialnumb_next = serialnumb + i;

								// check trùng số tem
								var existed1 = existedAccreditations
									.FirstOrDefault(e => e.SerialNumber == serialnumb_next);

								if (existed1 != null)
								{
									string retKey1 = "stamp_number_existed_next";
									if (userDict.TryGetValue(existed1.Tester1Id ?? 0, out var tester1) &&
										equipmentDict.TryGetValue(existed1.equiment.Id, out var chkContractStamp1))
									{
										retKey1 += "#" + $"Số seri: {serialnumb_next} do KĐV {tester1.DisplayName}" +
												   $" - Tại hợp đồng số: {chkContractStamp1.contract.MaHD}" +
												   $" - Tên TB: {chkContractStamp1.Name}";
									}
									return Json(retKey1, JsonRequestBehavior.AllowGet);
								}

								// kiểm tra phạm vi tem
								var temKDV1 = existedStamps.FirstOrDefault(e => e.SerialNumber == serialnumb_next);

								if (temKDV1 == null)
								{
									string retKey2 = "invaild_stamp_not";
									retKey2 += "#" + $"Số tem: {serialnumb_next:0000000} chưa được giao cho KĐV nào.";
									return Json(retKey2, JsonRequestBehavior.AllowGet);
								}

								if (temKDV1.OwnerId != accreditation.Tester1Id &&
									temKDV1.OwnerId != accreditation.Tester2Id)
								{
									string retKey3 = "stamp_number_out";
									if (userDict.TryGetValue(temKDV1.OwnerId, out var testerStamp))
									{
										retKey3 += "#" + $"{serialnumb_next:0000000} đã giao cho KĐV {testerStamp.DisplayName}";
									}
									return Json(retKey3, JsonRequestBehavior.AllowGet);
								}
							}

							// ==== Update accreditation ====
							if (acc != null)
							{
								if (acc.Id == accreditation.Id || !string.IsNullOrWhiteSpace(acc.NumberAcc))
									continue;

								acc.NumberAcc = sokqkds[sokqInx++];
								acc.AccrLocation = accreditation.AccrLocation;
								acc.Location = accreditation.Location;
								acc.DateOfNext = accreditation.DateOfNext;
								acc.AccrDate = accreditation.AccrDate;
								acc.AccrResultDate = accreditation.AccrResultDate;
								acc.Viewer1 = accreditation.Viewer1;
								acc.PositionViewer1 = accreditation.PositionViewer1;
								acc.Viewer2 = accreditation.Viewer2;
								acc.PositionViewer2 = accreditation.PositionViewer2;
								acc.PartionsNotice = accreditation.PartionsNotice;
								acc.LoadTestNotice = accreditation.LoadTestNotice;
								acc.StampLocated = accreditation.StampLocated;
								acc.Tester1Id = accreditation.Tester1Id;
								acc.Tester2Id = accreditation.Tester2Id;
								acc.TypeAcc = accreditation.TypeAcc;
								acc.EmployedProcedure = accreditation.EmployedProcedure;
								acc.EmployedStandard = accreditation.EmployedStandard;
								acc.Ma_TP = provinceIdstr;
								acc.Ma_QH = provinceQH_Idstr;
								acc.Ma_PX = provincePX_Idstr;

								if (!string.IsNullOrEmpty(formValues["StampNumber"]))
								{
									var inpStampNumber = formValues["StampNumber"].Trim();
									if (int.TryParse(inpStampNumber, out int stampnumb))
									{
										acc.StampNumber = (stampnumb + i).ToString();
										acc.SerialNumber = __getStampSerial(acc.AccTask.AccTaskNote, (stampnumb + i));
									}
								}

								uow.Repository<Accreditation>().Update(acc);
							}

							// ==== Ghi lại TSKT nếu đã nhập ở TB đầu
							//if (accreditation.AccTask.AccTaskNote == "KĐXD-TBN" || accreditation.AccTask.AccTaskNote == "KĐXD-TBAL")
							if (isSpec)
							{
								var spec = equipmentDict[accreditation.equipmentId];
								var equipTarget = equipmentDict[eq.Id];
								var existingSpecs = equipTarget.specifications ?? new List<Specifications>();

								foreach (var incomingSpec in spec.specifications)
								{
									var targetSpec = existingSpecs.FirstOrDefault(s => s.Name == incomingSpec.Name);
									if (targetSpec != null)
									{
										targetSpec.Value = incomingSpec.Value;
										targetSpec.f_key = incomingSpec.f_key;
										targetSpec.f_unit = incomingSpec.f_unit;
										uow.Repository<Specifications>().Update(targetSpec);
									}
									else
									{
										equipTarget.specifications.Add(new Specifications
										{
											Name = incomingSpec.Name,
											Value = incomingSpec.Value,
											f_key = incomingSpec.f_key,
											f_unit = incomingSpec.f_unit
										});
									}
								}
							}
							
						}

						// ==== 3. Lưu thay đổi một lần cuối ====
						uow.SaveChanges();
					}					
				}
				catch
				{

				}              

				EquipmentAddedList.EquipmentsAdded = new List<Equipment>();
				return Json("success", JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult CheckNumberAccreditationAvailable(string NumberAccre)
		{
			if (!string.IsNullOrWhiteSpace(NumberAccre))
			{
				var accre = uow.Repository<Accreditation>().FindBy(x => x.NumberAcc == NumberAccre);
				if (accre.Count > 0)
					return Json("NotAvailable", JsonRequestBehavior.AllowGet);
				else
					return Json("Available", JsonRequestBehavior.AllowGet);
			}
			else return Json("Available", JsonRequestBehavior.AllowGet);
		}

		public ActionResult RefresEquipmentData()
		{
			EquipmentDataProvider.equipments = null;
			EquipmentDataProvider.allequipments = null;
			return Json("success", JsonRequestBehavior.AllowGet);
		}

		public ActionResult AdvancedEquipmentFilterPartial()
		{
			if (User.IsInRole("Admin") || User.IsInRole("TPTH"))
			{
				ViewData["Departments"] = uow.Repository<Department>().GetAll();
				if (GridViewHelper.AdvancedEquipmentFilterDepartmentID > 0)
				{
					var department = uow.Repository<Department>().FindBy(x => x.Id == GridViewHelper.AdvancedEquipmentFilterDepartmentID).FirstOrDefault();

					ViewData["Employees"] = department != null ? uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Department != null && x.Department.Id == department.Id).ToList() : new List<Core.DomainModels.Identity.AppUser>();
				}
				else
				{
					ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
				}
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Departments"] = uow.Repository<Department>().FindBy(x => user != null && user.Department != null && x.Id == user.Department.Id).ToList();

				if (GridViewHelper.AdvancedEquipmentFilterDepartmentID > 0)
				{
					var department = uow.Repository<Department>().FindBy(x => x.Id == GridViewHelper.AdvancedEquipmentFilterDepartmentID).FirstOrDefault();

					ViewData["Employees"] = department != null ? uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Department != null && x.Department.Id == department.Id).ToList() : new List<Core.DomainModels.Identity.AppUser>();
				}
				else
				{
					ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
				}
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				ViewData["Departments"] = uow.Repository<Department>().FindBy(x => user != null && user.Department != null && x.Id == user.Department.Id).ToList();

				if (GridViewHelper.AdvancedEquipmentFilterDepartmentID > 0)
				{
					var department = uow.Repository<Department>().FindBy(x => x.Id == GridViewHelper.AdvancedEquipmentFilterDepartmentID).FirstOrDefault();

					ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>() { user };
				}
				else
				{
					ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
				}
			}

			return PartialView("_AdvancedEquipmentFilterPartial");
		}
		[HttpPost]
		public ActionResult AdvancedEquipmentFilterPartial(string FromDate, string ToDate, int DepartmentID, int EmployeeID, string DepartmentName, string EmployeeName)
		{
			GridViewHelper.IsAdvancedEquipmentFilterFromDate = true;

			if (DateTime.TryParseExact(FromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _fromDate))
				GridViewHelper.AdvancedEquipmentFilterFromDate = _fromDate;
			if (DateTime.TryParseExact(ToDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime _toDate))
				GridViewHelper.AdvancedEquipmentFilterToDate = _toDate;

			if (User.IsInRole("Admin") || User.IsInRole("TPTH"))
			{
				GridViewHelper.AdvancedEquipmentFilterDepartmentID = DepartmentID;
				GridViewHelper.AdvancedEquipmentFilterEmployeeID = EmployeeID;
				GridViewHelper.AdvancedEquipmentFilterDepartmentName = string.IsNullOrEmpty(DepartmentName) ? "N/A" : DepartmentName;
				GridViewHelper.AdvancedEquipmentFilterEmployeeName = string.IsNullOrEmpty(EmployeeName) ? "N/A" : EmployeeName;
			}
			else if (User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
			{
				var user = userManager.FindByName(User.Identity.Name);
				GridViewHelper.AdvancedEquipmentFilterDepartmentID = user.Department != null ? user.Department.Id : 0;
				GridViewHelper.AdvancedEquipmentFilterEmployeeID = EmployeeID;
				GridViewHelper.AdvancedEquipmentFilterDepartmentName = user != null && user.Department != null ? user.Department.Name : "N/A";
				GridViewHelper.AdvancedEquipmentFilterEmployeeName = string.IsNullOrEmpty(EmployeeName) ? "N/A" : EmployeeName;
			}
			else
			{
				var user = userManager.FindByName(User.Identity.Name);
				GridViewHelper.AdvancedEquipmentFilterDepartmentID = user.Department != null ? user.Department.Id : 0;
				GridViewHelper.AdvancedEquipmentFilterEmployeeID = user != null ? user.Id : 0;
				GridViewHelper.AdvancedEquipmentFilterDepartmentName = user != null && user.Department != null ? user.Department.Name : "N/A";
				GridViewHelper.AdvancedEquipmentFilterEmployeeName = user != null ? user.DisplayName : "N/A";
			}

			return Json("success", JsonRequestBehavior.AllowGet);
		}

		public ActionResult EmployeesCallbackRouteValues()
		{
			int departmentID = (!string.IsNullOrWhiteSpace(Request.Params["DepartmentID"])) ? int.Parse(Request.Params["DepartmentID"]) : -1;
			if (departmentID > 0)
			{
				var department = uow.Repository<Department>().FindBy(x => x.Id == departmentID).FirstOrDefault();

				if (User.IsInRole("Admin") || User.IsInRole("TPTH") || User.IsInRole("DeptDirector") || User.IsInRole("Accountant"))
					ViewData["Employees"] = department != null ? uow.Repository<Core.DomainModels.Identity.AppUser>().FindBy(x => x.Department != null && x.Department.Id == department.Id).ToList() : new List<Core.DomainModels.Identity.AppUser>();
				else
				{
					var user = userManager.FindByName(User.Identity.Name);
					ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>() { user };
				}
			}
			else
			{
				ViewData["Employees"] = new List<Core.DomainModels.Identity.AppUser>();
			}
			return PartialView();
		}

		public ActionResult RemoveAdvancedEquipmentFilterPartial()
		{
			GridViewHelper.IsAdvancedEquipmentFilterFromDate = false;
			return Json("success", JsonRequestBehavior.AllowGet);
		}
		private List<string> GetTestingResultNumber(int ContractID, int TaskId, int num)
		{
			var res = new List<string>();

			return res;
		}

		/*
		 Note by Lapbt
		 Event: 
			- Gọi vào đây khi nhấn nút "Lấy số" ở trong form "Chỉnh sửa biên bản" (sau khi gọi ở chỗ "Tạo thiết bị")
		 */
		public ActionResult GetNewTestingResultNumber_1(int ContractID, int TaskId)
		{			 
			var newShd = __getNewTestingResultNumber_1(ContractID, TaskId);
			return Json(newShd, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public ActionResult GetNewTestingResultNumber(int ContractID, int TaskId, DateTime AccrDate)
		{
			var newShd = __getNewTestingResultNumber(ContractID, TaskId, AccrDate);
			return Json(newShd);
		}

		public ActionResult OnCapSoKQKDFailure(int ContractID, int TaskId, int accId)
		{
			var contract = uow.Repository<Contract>().FindBy(x => x.Id == ContractID).FirstOrDefault();
			var task = contract?.Tasks.FirstOrDefault(x => x.Id == TaskId);

			var accs = uow.Repository<Accreditation>().FindBy(e => e.AccTask_Id == TaskId && string.IsNullOrEmpty(e.NumberAcc));

			if (accs.Count > 0)
			{
				var del_null = false;
				foreach (var a in accs)
				{
					// Xoa cac dong co NumberAcc == Null
					if (a.NumberAcc == null)
					{
						System.Diagnostics.Debug.WriteLine(a.Id);
						System.Diagnostics.Debug.WriteLine(a.NumberAcc);
						uow.Repository<Accreditation>().Delete(a);
						del_null = true;
					}
				}

				if (del_null)
				{
					uow.SaveChanges();
				}
			}

			// --- Thêm xử lý xóa thiết bị không còn liên kết ---
			// Lấy toàn bộ AccTask.Id thuộc contract hiện tại				
			var accTaskIds = contract?.Tasks.Select(t => t.Id).ToList();
			// Lấy toàn bộ equipment_Id trong bảng Accreditation ứng với các accTaskIds đó
			var accEquipIds = uow.Repository<Accreditation>()
				.FindBy(e => accTaskIds.Contains(e.AccTask.Id))
				.Select(e => e.equiment.Id)
				.ToList();

			// Lấy thiết bị thuộc contract nhưng không có trong danh sách accEquipIds
			var equipmentsToDelete = uow.Repository<Equipment>()
				.FindBy(eq => eq.contract.Id == ContractID && !accEquipIds.Contains(eq.Id))
				.ToList();

			if (equipmentsToDelete.Any())
			{
				foreach (var eq in equipmentsToDelete)
				{
					// 🔹 Xóa tất cả Specifications trước
					var specs = uow.Repository<Specifications>()
						.FindBy(s => s.Equipment.Id == eq.Id)
						.ToList();

					foreach (var sp in specs)
					{
						uow.Repository<Specifications>().Delete(sp);
					}

					// 🔹 Sau đó mới xóa Equipment
					uow.Repository<Equipment>().Delete(eq);
				}
				uow.SaveChanges();
			}
			return Json("success", JsonRequestBehavior.AllowGet);
		}


		private string __getNewTestingResultNumber(int ContractID, int TaskId, DateTime accrDate)
		{
			Dictionary<string, int> InitSoKQKD = new Dictionary<string, int>()
	{
		{ "KĐXD-TBN", 0 },
		{ "KĐXD-TBAL", 0 },
		{ "KĐXD-TBTC", 0 },
		{ "KĐXD-HQ", 0 },
		{ "KĐXD-AK", 0 },
		{ "KĐXD-HL", 0 },
		{ "KĐXD-TVXD", 0 },
		{ "KĐXD-TNLAS", 0 },
		{ "KĐXD-TNNOLAS", 0 },
		{ "KĐXD-TNHT", 0 },
		{ "KĐXD-TNPTN", 0 }
	};

			var contract = uow.Repository<Contract>().GetSingle(ContractID);
			if (contract == null) return "";

			var task = contract.Tasks.FirstOrDefault(e => e.Id == TaskId);
			if (task == null) return "";

			// 👉 prefix theo NĂM KIỂM ĐỊNH
			int prefixYear = accrDate.Year;
			//Nếu năm của ngày KĐ > năm hiện hành thì lấy bằng năm hiện hành
			if (prefixYear > DateTime.Now.Year)
				prefixYear = DateTime.Now.Year;

			var prefix = (prefixYear % 100).ToString("00");

			Regex regex = new Regex(@"(\d+)(?=/" + Regex.Escape(task.AccTaskNote) + @")");

			var thisYearAccs = uow.Repository<Accreditation>()
				.FindBy(e => !string.IsNullOrEmpty(e.NumberAcc)
					&& e.NumberAcc.StartsWith(prefix)
					&& e.NumberAcc.EndsWith(task.AccTaskNote))
				.ToList();

			var mahds = thisYearAccs
				.Select(e =>
				{
					var match = regex.Match(e.NumberAcc);
					return match.Success && int.TryParse(match.Groups[1].Value, out int r) ? r : 0;
				})
				.ToList();

			var soHD = mahds.Count > 0 ? mahds.Max() : 0;

			// Số đầu năm theo chi nhánh; HN lấy đầu = 0
			if (soHD == 0)
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user.Department.Id == 8) soHD = 35000;   // HCM
				if (user.Department.Id == 10) soHD = 50000; // ĐN
			}

			var newShd = prefix + "." + (soHD + 1).ToString("00000") + "/" + task.AccTaskNote;

			while (thisYearAccs.Any(e => e.NumberAcc == newShd))
			{
				soHD++;
				newShd = prefix + "." + (soHD + 1).ToString("00000") + "/" + task.AccTaskNote;
			}

			return newShd;
		}

		/*
		 Note by lapbt
		 Event: Click (b3.1 ~ b3 gọi sang đây) nhấn nút lấy số BBKD 
			- Tao so thiet: cho 1 thiet bi dau tien (cac thiet bi sau van de null)
				- Ndung: đg dò lỗi k0 đc số BBKD khi ds cv bên phải k0 reload lên đc
		 */
		private string __getNewTestingResultNumber_1(int ContractID, int TaskId)
		{
			Dictionary<string, int> InitSoKQKD = new Dictionary<string, int>();
			
			InitSoKQKD.Add("KĐXD-TBN", 0);
			InitSoKQKD.Add("KĐXD-TBAL", 0);
			InitSoKQKD.Add("KĐXD-TBTC", 0);
			InitSoKQKD.Add("KĐXD-HQ", 0);
			InitSoKQKD.Add("KĐXD-AK", 0);
			InitSoKQKD.Add("KĐXD-HL", 0);
			InitSoKQKD.Add("KĐXD-TVXD", 0);
			InitSoKQKD.Add("KĐXD-TNLAS", 0);
			InitSoKQKD.Add("KĐXD-TNNOLAS", 0);
			InitSoKQKD.Add("KĐXD-TNHT", 0);
			InitSoKQKD.Add("KĐXD-TNPTN", 0);

			var contract = uow.Repository<Contract>().GetSingle(ContractID);

			if (contract != null)
			{
				var task = contract.Tasks.FirstOrDefault(e => e.Id == TaskId);
				var manv = contract.KDV1 == null ? contract.own.MaNV : contract.KDV1.MaNV;				

				//04.06.2025 Thay đổi định dạng số KQKĐ (bỏ mã MV) nên cần thay code lấy số KQKD lớn nhất
				// Tạo regex để lấy số cuối cùng trước dấu /
				Regex regex = new Regex(@"(\d+)(?=/" + Regex.Escape(task.AccTaskNote) + @")");
				int nowYear = DateTime.Today.Year + 1 - 2000;
				//var prefix = DateTime.Today.ToString("yy");
				var prefix = nowYear.ToString();

				var thisYearAccs = uow.Repository<Accreditation>()
					.FindBy(e => !string.IsNullOrEmpty(e.NumberAcc)
						&& e.NumberAcc.StartsWith(prefix)
						&& e.NumberAcc.EndsWith(task.AccTaskNote))
					.ToList();
				// Tách số từ cuối chuỗi trước /task.AccTaskNote
				var mahds = thisYearAccs
					.Select(e => {
						var match = regex.Match(e.NumberAcc);
						if (match.Success)
						{
							int result;
							return Int32.TryParse(match.Groups[1].Value, out result) ? result : 0;
						}
						return 0;
					})
					.ToList();
				// Lấy số lớn nhất
				//29.12.2025 Cần sửa chỗ này để lấy số đầu năm: HN bắt đầu từ 00001; HCM: từ 35001; ĐN từ 50001
				var soHD = mahds.Count > 0 ? mahds.Max().ToString() : "0";
				
				if (soHD == "0")
                {
					var user = userManager.FindByName(User.Identity.Name);
					if (user.Department.Id == 8) soHD = "35000";

					if (user.Department.Id == 10) soHD = "50000";
				}	

				var soHdInt = string.IsNullOrWhiteSpace(soHD)? 0: int.Parse(soHD);
				if (soHdInt == 0 && InitSoKQKD.ContainsKey(task.AccTaskNote))
					soHdInt = InitSoKQKD[task.AccTaskNote] - 1;

				if (soHdInt < 0) soHdInt = 0;
				
				var newShd = prefix + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
				while (thisYearAccs.Any(e => e.NumberAcc == newShd))
				{
					soHdInt += 1;					
					newShd = prefix + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
				}
				return newShd;

			}
			return "";
		}

		/*
		 Note by lapbt
		Tao so thiet: cho cac thiet bi so co la null con lai
		Goi sau ham: private string __getNewTestingResultNumber(int ContractID, int TaskId)
		 */
		private List<string> __getNewTestingResultNumber(int ContractID, int TaskId, int numsCount,	DateTime accrDate)
		{
			var res = new List<string>();
			var contract = uow.Repository<Contract>().GetSingle(ContractID);
			if (contract == null || numsCount <= 0) return res;

			var task = contract.Tasks.FirstOrDefault(e => e.Id == TaskId);
			if (task == null) return res;

			Dictionary<string, int> InitSoKQKD = new Dictionary<string, int>()
	{
		{ "KĐXD-TBN", 0 },
		{ "KĐXD-TBAL", 0 },
		{ "KĐXD-TBTC", 0 },
		{ "KĐXD-HQ", 0 },
		{ "KĐXD-AK", 0 },
		{ "KĐXD-HL", 0 },
		{ "KĐXD-TVXD", 0 },
		{ "KĐXD-TNLAS", 0 },
		{ "KĐXD-TNNOLAS", 0 },
		{ "KĐXD-TNHT", 0 },
		{ "KĐXD-TNPTN", 0 }
	};

			// 👉 prefix theo NĂM KIỂM ĐỊNH
			var prefix = (accrDate.Year % 100).ToString("00");
			//Nếu năm của ngày KĐ > năm hiện hành thì lấy bằng năm hiện hành
			if (accrDate.Year > DateTime.Now.Year)
				prefix = (DateTime.Now.Year % 100).ToString("00");

			Regex regex = new Regex(@"(\d+)(?=/" + Regex.Escape(task.AccTaskNote) + @")");

			var thisYearAccs = uow.Repository<Accreditation>()
				.FindBy(e => !string.IsNullOrEmpty(e.NumberAcc)
					&& e.NumberAcc.StartsWith(prefix)
					&& e.NumberAcc.EndsWith(task.AccTaskNote))
				.ToList();

			var maxNumber = thisYearAccs
				.Select(e =>
				{
					var match = regex.Match(e.NumberAcc);
					return match.Success && int.TryParse(match.Groups[1].Value, out int r) ? r : 0;
				})
				.DefaultIfEmpty(0)
				.Max();

			// 👉 Nếu đầu năm chưa có số → set theo chi nhánh
			if (maxNumber == 0)
			{
				var user = userManager.FindByName(User.Identity.Name);
				if (user.Department.Id == 8) maxNumber = 35000;   // HCM
				if (user.Department.Id == 10) maxNumber = 50000; // ĐN
			}

			// 👉 Sinh numsCount số liên tiếp
			for (int i = 1; i <= numsCount; i++)
			{
				var newShd = prefix + "."
					+ (maxNumber + i).ToString("00000")
					+ "/"
					+ task.AccTaskNote;

				// đảm bảo không trùng (an toàn)
				while (thisYearAccs.Any(e => e.NumberAcc == newShd) || res.Contains(newShd))
				{
					maxNumber++;
					newShd = prefix + "."
						+ (maxNumber + i).ToString("00000")
						+ "/"
						+ task.AccTaskNote;
				}

				res.Add(newShd);
			}

			return res;
		}
		private List<string> __getNewTestingResultNumber_1(int ContractID, int TaskId, int numsCount)
		{
			var res = new List<string>();
			var contract = uow.Repository<Contract>().GetSingle(ContractID);

			Dictionary<string, int> InitSoKQKD = new Dictionary<string, int>();
			
			InitSoKQKD.Add("KĐXD-TBN", 1);
			InitSoKQKD.Add("KĐXD-TBAL", 1);
			InitSoKQKD.Add("KĐXD-TBTC", 1);
			InitSoKQKD.Add("KĐXD-HQ", 1);
			InitSoKQKD.Add("KĐXD-AK", 1);
			InitSoKQKD.Add("KĐXD-HL", 1);
			InitSoKQKD.Add("KĐXD-TVXD", 1);
			InitSoKQKD.Add("KĐXD-TNLAS", 1);
			InitSoKQKD.Add("KĐXD-TNNOLAS", 1);
			InitSoKQKD.Add("KĐXD-TNHT", 1);
			InitSoKQKD.Add("KĐXD-TNPTN", 1);

			if (contract != null)
			{
				var task = contract.Tasks.FirstOrDefault(e => e.Id == TaskId);
				var manv = contract.KDV1 == null ? contract.own.MaNV : contract.KDV1.MaNV;				

				//04.06.2025 Thay đổi định dạng số KQKĐ (bỏ mã MV) nên cần thay code lấy số KQKD lớn nhất
				Regex regex = new Regex(@"(\d+)(?=/" + Regex.Escape(task.AccTaskNote) + @")");
				var prefix = DateTime.Today.ToString("yy");				
				var thisYearAccs = uow.Repository<Accreditation>()
					.FindBy(e => !string.IsNullOrEmpty(e.NumberAcc)
						&& e.NumberAcc.StartsWith(prefix)
						&& e.NumberAcc.EndsWith(task.AccTaskNote))
					.ToList();
				// Tách số từ cuối chuỗi trước /task.AccTaskNote
				var mahds = thisYearAccs
					.Select(e => {
						var match = regex.Match(e.NumberAcc);
						if (match.Success)
						{
							int result;
							return Int32.TryParse(match.Groups[1].Value, out result) ? result : 0;
						}
						return 0;
					})
					.ToList();
				// Lấy số lớn nhất
				var soHD = mahds.Count > 0 ? mahds.Max().ToString() : "0";

				var soHdInt = string.IsNullOrWhiteSpace(soHD) ? 0 : int.Parse(soHD);
				if (soHdInt == 0 && InitSoKQKD.ContainsKey(task.AccTaskNote))
					soHdInt = InitSoKQKD[task.AccTaskNote] - 1;

				//26.05.2025 Hung bỏ maNV trong số KQKĐ
				//var newShd = prefix + manv + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
				var newShd = prefix + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
				while (thisYearAccs.Any(e => e.NumberAcc == newShd))
				{
					soHdInt += 1;
					//newShd = prefix + manv + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
					newShd = prefix + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
				}
				res.Add(newShd);

				for (int i = 0; i < numsCount-1; i++)
				{
					soHdInt += 1;
					//newShd = prefix + manv + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
					newShd = prefix + "." + (soHdInt + 1).ToString("#00000") + "/" + task.AccTaskNote;
					res.Add(newShd);
				}

			}
			return res;
		}
		public ActionResult ExportTo(GridViewExportFormat? exportFormat)
		{
			if (exportFormat == null || !GridViewExportHelper.ExportFormatsInfo.ContainsKey(exportFormat.Value))
				return RedirectToAction("Equipment", "Home");

			return GridViewExportHelper.ExportFormatsInfo[exportFormat.Value](
				GridViewToolbarHelper.ExportGridSettings, EquipmentDataProvider.AllEquipments
			);
		}


		/*
		 * 19-mar-2024. Added by lapbt
		 * Hàm này để nhận gọi từ form Sửa BBKD -> Chọn Tỉnh/Thành -> lấy ds QH trả về để build lên combo QH
		 * Cách làm load động chỗ này như sau
		 * - Ở giao diện: Views\Equipments\AccreditationViewViaContract.cshtml
		 *      - thêm combo cha cần bắt sự kiện
		 *          settings.Properties.ClientSideEvents.SelectedIndexChanged = "function () { cmbProvinces_QH.PerformCallback(); }";
		 *          settings.Properties.ClientSideEvents.ValueChanged = "function () { cmbProvinces_QH.PerformCallback(); }";
		 *        để gọi vào sự kiện callback của combo con
		 *        
		 *      - ở combo con, có ứng phó
		 *          settings.Properties.ClientSideEvents.BeginCallback = "function(s, e) { e.customArgs['Ma_TP'] = cmbProvinces.GetValue(); }";
		 *          settings.CallbackRouteValues = new { Action = "Provinces_QH_CallbackRouteValues", Controller = "Equipments" };
		 *        để lấy giá trị chọn ở combo cha -> gọi tới controller để lấy DL
		 *        
		 * - Ở controller này
		 *      - thêm control nghe: ActionResult Provinces_QH_CallbackRouteValues()
		 *      - tạo ra 1 partial view: Views\Equipments\_Provinces_QH_CallbackRouteValuesParital
		 *          với giá trị như khúc ở bên giao diện, nhưng bỏ đi khúc modal để k0 khởi tạo giá trị default, cho để trống
		 *          chỉ khởi tạo ComboBox thông thường, k0 cần ComboBoxFor khi sử dụng modal
		 *      - lấy DL xong trả về cho partial
		 */
		public ActionResult Provinces_QH_CallbackRouteValues()
		{
			var ma_TP = Request.Params["Ma_TP"];
			if (!string.IsNullOrWhiteSpace(ma_TP))
			{
				ViewData["ProvincesFull_QH"] = uow.Repository<Core.DomainModels.v_ProvinceDistrict>().FindBy(x => x.Ma_TP == ma_TP).ToList();
			}
			else
			{
				ViewData["ProvincesFull_QH"] = new List<Core.DomainModels.v_ProvinceDistrict>();
			}
			return PartialView("_Provinces_QH_CallbackRouteValuesParital");
		}

		public ActionResult Provinces_QH_PX_CallbackRouteValues()
		{
			var ma_TP = Request.Params["Ma_TP"];
			var ma_QH = Request.Params["Ma_QH"];
			if (!string.IsNullOrWhiteSpace(ma_QH))
			{
				ViewData["ProvincesFull_QH_PX"] = uow.Repository<Core.DomainModels.ProvinceFull>().FindBy(x => x.Ma_TP == ma_TP && x.Ma_QH == ma_QH).ToList();
			}
			else
			{
				ViewData["ProvincesFull_QH_PX"] = new List<Core.DomainModels.ProvinceFull>();
			}
			return PartialView("_Provinces_QH_PX_CallbackRouteValuesParital");
		}

		
		[HttpPost]
		//19.06.2025 Lưu số liệu nhập TSKT trực tiếp trên lưới grid khi lấy số KQKĐ
		public ActionResult SpecificationsOfEquipmentBatchUpdate(MVCxGridViewBatchUpdateValues<Specifications, int> updateValues)
		{
			try
			{
				// xử lý dữ liệu
				foreach (var item in updateValues.Update)
				{
					if (ModelState.IsValid)
					{
						// Cập nhật dữ liệu
						var entity = uow.Repository<Specifications>().FindBy(x => x.Id == item.Id).First();						
						if (entity != null)
						{
							entity.Name = item.Name;
							entity.Value = item.Value;							
							//entity.f_key = item.f_key;
							entity.f_unit = item.f_unit;
							// ... thêm các trường khác nếu cần
						}						
					}

				}
				uow.SaveChanges();
				
				var spec = uow.Repository<Equipment>().GetSingle(e => e.Id == GridViewHelper.SelectedEquipmentID);				
				var list = spec.specifications ?? new List<Specifications>();

				// Sắp xếp cố định theo thứ tự id ban đầu
				var orderedList = list.OrderBy(x => x.Id).ToList();

				return PartialView("~/Views/Equipments/_SpecificationsOfEquipmentEditPartial.cshtml", orderedList);
			}
			catch (Exception ex)
			{
				return Content("ERROR: " + ex.Message);
			}		

		}

		//4.7.2025 Hưng thêm Danh sách thiết bị của Hợp đồng/GĐN
		//Cần sửa lại lọc TB theo KDV/toàn bộ; theo HĐ hiện tại/toàn bộ ở proc .... (đang làm dở)
		/*
		[ValidateInput(false)]
		public ActionResult EquipmentOfContractPartial(int contractId)
		{
			string selectedUserId = "12";
			string selectedLoaiHinh = "KĐAT";
			var currentUser = userManager.FindByName(User.Identity.Name);
			bool isAdmin = User.IsInRole("Admin");
			bool isDeptDirector = User.IsInRole("DeptDirector");
			ViewBag.SelectedUserId = selectedUserId;
			ViewBag.SelectedLoaiHinh = selectedLoaiHinh;

			// Lấy danh sách nhân viên
			if (isAdmin)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();

				userList.Insert(0, new SelectListItem { Value = "", Text = "Toàn bộ" });
				ViewBag.UserList = userList;
			}

			if (isDeptDirector)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled && x.Department.Id == currentUser.Department.Id)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();

				userList.Insert(0, new SelectListItem { Value = "", Text = "Toàn Đơn vị" });
				ViewBag.UserList = userList;
			}

			if (!isAdmin && !isDeptDirector)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled && x.Id == currentUser.Id)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();
				ViewBag.UserList = userList;
			}

			var equipments = EquipmentDataProvider.AllEquipments
				.Where(e => e.ContractID == contractId)
				.OrderByDescending(e => e.ContractSignDate)
				.ToList();

			// Lấy toàn bộ thiết bị ID để truy vấn Specifications
			var equipmentIds = equipments.Select(e => e.Id).ToList();

			// Lấy specifications cho các thiết bị
			var specsByEquipId = uow.Repository<Specifications>()
				.FindBy(s => equipmentIds.Contains(s.Equipment.Id)) // hoặc s.Equipment_Id nếu có
				.GroupBy(s => s.Equipment.Id)
				.ToDictionary(g => g.Key, g => g.FirstOrDefault());

			// Gán FirstSpecDisplay vào từng thiết bị
			foreach (var e in equipments)
			{
				if (specsByEquipId.TryGetValue(e.Id, out var firstSpec) && firstSpec.Value != null)
				{
					e.FirstSpecDisplay = $"{firstSpec.Name}: {firstSpec.Value} {firstSpec.f_unit}";
				}
				else
				{
					e.FirstSpecDisplay = "";
				}
			}

			return PartialView("_EquipmentOfContractPartial", equipments);
		}
		*/

		//code mới 12.8.2025
		//đổi tên thành ActionResult EquipmentOfContractPartialOld (tên cũ EquipmentOfContractPartial) 
		[ValidateInput(false)]
		public ActionResult EquipmentOfContractPartialOld(int contractId, int? userId, string selectedLoaiHinh, string scope)
		{			
			var currentUser = userManager.FindByName(User.Identity.Name);
			bool isAdmin = User.IsInRole("Admin");
			bool isDeptDirector = User.IsInRole("DeptDirector");
			if (!isAdmin)
			{
				userId = currentUser.Id;
			}
			if (string.IsNullOrEmpty(selectedLoaiHinh))
				selectedLoaiHinh = "";

			if (string.IsNullOrEmpty(scope))
				scope = "Curren";
			
			if (scope == "All")
				contractId = 0;

			if (contractId == 0)
				scope = "All";

			ViewBag.SelectedUser_Id = userId.ToString();
			ViewBag.SelectedLoaiHinh = selectedLoaiHinh;
			ViewBag.SelectedScope = scope;

			// Lấy danh sách nhân viên
			if (isAdmin)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();

				userList.Insert(0, new SelectListItem { Value = "", Text = "KĐV: Toàn bộ" });
				ViewBag.UserList = userList;
			}
			/*
			if (isDeptDirector)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled && x.Department.Id == currentUser.Department.Id)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();

				userList.Insert(0, new SelectListItem { Value = "", Text = "Toàn Đơn vị" });
				ViewBag.UserList = userList;
			}
			*/
			if (!isAdmin)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled && x.Id == currentUser.Id)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();
				ViewBag.UserList = userList;
			}
			
			var equipments = EquipmentDataProvider
							.GetEquipmentsByUser(
							DateTime.Today.AddYears(-3),
							DateTime.Today.AddYears(1),
							isAdmin ? (int?)null : userId,
							isAdmin,
							contractId == 0 ? (int?)null : contractId
							)
							.OrderByDescending(e => e.ContractSignDate)
							.ToList();			

			// Filter theo KĐV			
			if (userId.HasValue && userId.Value > 0)
				equipments = equipments.Where(x => x.OwnerID == userId.Value).ToList();
			
			// Filter theo Loại hình
			if (!string.IsNullOrEmpty(selectedLoaiHinh))
			{
				if (selectedLoaiHinh == "1")
					equipments = equipments.Where(x => x.AccTaskType == "KĐXD-TBN" || x.AccTaskType == "KĐXD-TBAL").ToList();
				else if (selectedLoaiHinh == "2")
					equipments = equipments.Where(x => x.AccTaskType == "KĐXD-AK").ToList();
				else if (selectedLoaiHinh == "3")
					equipments = equipments.Where(x => x.AccTaskType == "KĐXD-HQ").ToList();
				else if (selectedLoaiHinh == "4")
					equipments = equipments.Where(x => x.AccTaskType == "KĐXD-TNHT").ToList();
				else if (selectedLoaiHinh == "5")
					equipments = equipments.Where(x => x.AccTaskType == "KĐXD-TNPTN").ToList();
			}

			// Filter DS thiết bị theo HĐ hiện tại hoặc toàn bộ TB
			if (string.IsNullOrEmpty(scope) || scope == "Curren")
			{
				if (contractId > 0)
					equipments = equipments.Where(x => x.ContractID == contractId).ToList();
			}
			
			ViewBag.ContractId = contractId;

			// Lấy Specifications
			var equipmentIds = equipments.Select(e => e.Id).ToList();
			var specsByEquipId = uow.Repository<Specifications>()
				.FindBy(s => equipmentIds.Contains(s.Equipment.Id))
				.GroupBy(s => s.Equipment.Id)
				.ToDictionary(g => g.Key, g => g.FirstOrDefault());

			foreach (var e in equipments)
			{
				if (specsByEquipId.TryGetValue(e.Id, out var firstSpec) && firstSpec.Value != null)
				{
					e.FirstSpecDisplay = $"{firstSpec.Name}: {firstSpec.Value} {firstSpec.f_unit}";
				}
				else
				{
					e.FirstSpecDisplay = "";
				}
			}
			//ViewBag.ContractId = contractId;
			return PartialView("_EquipmentOfContractPartial", equipments);
		}

		//12.10.2025 Chuyển thanh tìm kiếm lên thành ô textbox ở thanh toolbar
		[ValidateInput(false)]
		public ActionResult EquipmentOfContractPartial(
							int contractId,
							int? userId,
							string selectedLoaiHinh,
							string scope,
							int? times_year,
							string search = ""      // 👈 thêm tham số search
						)
		{
			var currentUser = userManager.FindByName(User.Identity.Name);
			bool isAdmin = User.IsInRole("Admin");
			//bool isDeptDirector = User.IsInRole("DeptDirector");

			if (!isAdmin)
				userId = currentUser.Id;

			if (string.IsNullOrEmpty(selectedLoaiHinh))
				selectedLoaiHinh = "";

			if (string.IsNullOrEmpty(scope))
				scope = "Curren";

			if (scope == "All")
				contractId = 0;

			if (contractId == 0)
				scope = "All";			

			// ⚙️ Mặc định số năm nếu chưa chọn
			int years = (times_year.HasValue && times_year.Value > 0) ? times_year.Value : 2;
			if (times_year == 0 || scope == "Curren") years = 100;//Không giới hạn

		   ViewBag.SelectedUser_Id = userId.ToString();
			ViewBag.SelectedLoaiHinh = selectedLoaiHinh;
			ViewBag.SelectedScope = scope;
			ViewBag.SelectedTimes = years;

			// --- Lấy danh sách nhân viên ---
			if (isAdmin)
			{
				var userList = uow.Repository<AppUser>()
					.FindBy(x => x.TwoFactorEnabled)
					.Select(x => new SelectListItem
					{
						Value = x.Id.ToString(),
						Text = x.DisplayName
					})
					.ToList();

				userList.Insert(0, new SelectListItem { Value = "", Text = "KĐV: Toàn bộ" });
				ViewBag.UserList = userList;
			}
			else
			{
				var userList = uow.Repository<AppUser>()
					.FindBy(x => x.TwoFactorEnabled && x.Id == currentUser.Id)
					.Select(x => new SelectListItem
					{
						Value = x.Id.ToString(),
						Text = x.DisplayName
					})
					.ToList();
				ViewBag.UserList = userList;
			}

			// --- Dữ liệu thiết bị ---
			var equipments = EquipmentDataProvider
				.GetEquipmentsByUser(
					DateTime.Today.AddYears(-years),
					DateTime.Today.AddYears(1),
					isAdmin ? (int?)null : userId,
					isAdmin,
					contractId == 0 ? (int?)null : contractId
				)
				.OrderByDescending(e => e.ContractSignDate)
				.ToList();

			// --- Filter theo KĐV ---
			if (userId.HasValue && userId.Value > 0 && isAdmin)
				equipments = equipments.Where(x => x.OwnerID == userId.Value).ToList();

			// --- Filter theo loại hình ---
			if (!string.IsNullOrEmpty(selectedLoaiHinh))
			{
				switch (selectedLoaiHinh)
				{
					case "1": equipments = equipments.Where(x => x.AccTaskType == "KĐXD-TBN" || x.AccTaskType == "KĐXD-TBAL").ToList(); break;
					case "2": equipments = equipments.Where(x => x.AccTaskType == "KĐXD-AK").ToList(); break;
					case "3": equipments = equipments.Where(x => x.AccTaskType == "KĐXD-HQ").ToList(); break;
					case "4": equipments = equipments.Where(x => x.AccTaskType == "KĐXD-TNHT").ToList(); break;
					case "5": equipments = equipments.Where(x => x.AccTaskType == "KĐXD-TNPTN").ToList(); break;
				}
			}

			// --- Filter theo Hợp đồng ---
			if (string.IsNullOrEmpty(scope) || scope == "Curren")
			{
				if (contractId > 0)
					equipments = equipments.Where(x => x.ContractID == contractId).ToList();
			}

			// --- Filter theo Từ khóa tìm kiếm theo Tên TB/Tên ĐVSD/Nhà chế tạo/Số chế tạo/Số tem ---
			if (!string.IsNullOrWhiteSpace(search))
			{
				string keyword = search.Trim().ToLower();
				equipments = equipments.Where(x =>
					(x.Name != null && x.Name.ToLower().Contains(keyword)) ||
					(x.PartionsNotice != null && x.PartionsNotice.ToLower().Contains(keyword)) ||
					(x.ManuFacturer != null && x.ManuFacturer.ToLower().Contains(keyword)) ||
					(x.No != null && x.No.ToLower().Contains(keyword)) ||
					(x.StampNumber != null && x.StampNumber.ToLower().Contains(keyword))
				).ToList();
			}

			// --- Specifications ---
			/*Tạm đóng lấy TSKT cơ bản vào DS TB 13.10.2025
			var equipmentIds = equipments.Select(e => e.Id).ToList();
			var specsByEquipId = uow.Repository<Specifications>()
				.FindBy(s => equipmentIds.Contains(s.Equipment.Id))
				.GroupBy(s => s.Equipment.Id)
				.ToDictionary(g => g.Key, g => g.FirstOrDefault());

			foreach (var e in equipments)
			{
				if (specsByEquipId.TryGetValue(e.Id, out var firstSpec) && firstSpec.Value != null)
					e.FirstSpecDisplay = $"{firstSpec.Name}: {firstSpec.Value} {firstSpec.f_unit}";
				else
					e.FirstSpecDisplay = "";
			}
			*/
			ViewBag.ContractId = contractId;
			return PartialView("_EquipmentOfContractPartial", equipments);
		}


		//Ghi ở chế độ BatchEdit
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult EquipmentBatchUpdate(MVCxGridViewBatchUpdateValues<EquipmentViewModel, int> updateValues)
		{
			try
			{
				foreach (var item in updateValues.Update)
				{
					if (item == null) continue;

					var equipment = EquipmentDataProvider.DB.Equipments.Find(item.Id);
					if (equipment != null)
					{
						equipment.Name = item.Name;
						equipment.mahieu = item.mahieu;
						equipment.No = item.No;
						equipment.YearOfProduction = item.YearOfProduction;
						equipment.ManuFacturer = item.ManuFacturer;
						equipment.Code = item.Code;
						EquipmentDataProvider.DB.Entry(equipment).State = System.Data.Entity.EntityState.Modified;
					}

					var accreditation = EquipmentDataProvider.DB.Accreditations
						.FirstOrDefault(a => a.equiment.Id == item.Id);

					if (accreditation != null)
					{
						accreditation.AccrDate = item.AccreDate;
						accreditation.DateOfNext = item.NextAccreDate;
						accreditation.AccrResultDate = item.AccrResultDate;
						accreditation.StampNumber = item.StampNumber;
						accreditation.TypeAcc = (TypeOfAccr)item.TypeOfAccr;
						accreditation.Tester1Id = item.Tester1Id;
						accreditation.Tester2Id = item.Tester2Id;
						accreditation.Location = item.ViTriLapDat;
						accreditation.PartionsNotice = item.PartionsNotice;
						accreditation.LoadTestNotice = item.LoadTestNotice;

						EquipmentDataProvider.DB.Entry(accreditation).State = System.Data.Entity.EntityState.Modified;
					}
				}
				EquipmentDataProvider.DB.SaveChanges();
				// trả về chỉ partial của grid
				/*
				var equipments = EquipmentDataProvider.AllEquipments
					.Where(e => e.ContractID == GridViewHelper.SelectedContractID)
					.OrderByDescending(e => e.ContractSignDate)
					.ToList();
				*/
				var userId = userManager.FindByName(User.Identity.Name).Id;
				bool isAdmin = User.IsInRole("Admin");	
				var equipments = EquipmentDataProvider
				.GetEquipmentsByUser(
					DateTime.Today.AddYears(-3),
					DateTime.Today.AddYears(1),
					isAdmin ? (int?)null : userId,
					isAdmin,
					GridViewHelper.SelectedContractID == 0 ? (int?)null : GridViewHelper.SelectedContractID
				)
				.OrderByDescending(e => e.ContractSignDate)
				.ToList();

				ViewBag.SelectedScope = "Curren";
				return PartialView("~/Views/Equipments/_EquipmentOfContractPartial.cshtml", equipments);			
							
			}			
			catch (Exception ex)
			{
				return Content("ERROR: " + ex.Message);
			}
		}


		public ActionResult SpecificationsOfEquipmentEditByIdPartial(int equipmentId)
		{
			GridViewHelper.SelectedEquipmentID = equipmentId;
			var list = EquipmentDataProvider.GetSpecifications(equipmentId);
			return PartialView("_SpecificationsOfEquipmentEditPartial", list);
		}

		
		//9.7.2025 thêm 2 hàm sau để copy và paste TSKT			
		// GET: /Equipments/GetSpecifications
		public JsonResult GetSpecifications(int equipmentId)
		{
			var EquipCopy = uow.Repository<Equipment>().GetSingle(e => e.Id == equipmentId);
			var specs = EquipCopy.specifications ?? new List<Specifications>();			

			return Json(specs, JsonRequestBehavior.AllowGet);
		}

		// POST: /Equipments/PasteSpecifications
		[HttpPost]
		public JsonResult PasteSpecifications(int targetEquipmentId, List<Specifications> specs)
		{
			try
			{					
				var equipTarget = uow.Repository<Equipment>().GetSingle(e => e.Id == targetEquipmentId);

				// Xóa specs cũ
				foreach (var old in equipTarget.specifications.ToList())
				{
					uow.Repository<Specifications>().Delete(old);
				}

				// Thêm mới specs thông qua navigation
				foreach (var spec in specs)
				{
					var newSpec = new Specifications
					{
						Name = spec.Name,
						Value = spec.Value,
						f_key = spec.f_key,
						f_unit = spec.f_unit
					};
					equipTarget.specifications.Add(newSpec);  // 👈 dùng navigation
				}
				uow.SaveChanges();
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		//Thêm 11.07.2025 Xóa TB ở bảng Danh sách thiết bị theo hợp đồng
		[HttpPost]
		public ActionResult DeleteEquipmentCompletely(int id)
		{
			try
			{
				var equipment = uow.Repository<Equipment>().GetSingle(id);
				if (equipment == null)
					return Json("notfound", JsonRequestBehavior.AllowGet);

				// Xóa tất cả Accreditation liên quan
				var accreditations = uow.Repository<Accreditation>().FindBy(x => x.equiment != null && x.equiment.Id == id).FirstOrDefault();
				//Kiểm tra ngày khởi tạo > 5 so với hiện tại trước khi xóa
				var createdDate = accreditations?.CreateDate ?? DateTime.Today;
				TimeSpan difference = DateTime.Today - createdDate;				
				if (!string.IsNullOrEmpty(equipment.contract.MaHD) && difference.TotalDays > 5 && !User.IsInRole("Admin"))
					return Json("notDelete", JsonRequestBehavior.AllowGet);
				
				uow.Repository<Accreditation>().Delete(accreditations);

				// Xóa tất cả Specifications liên quan
				var specs = uow.Repository<Specifications>().FindBy(s => s.Equipment.Id == id).ToList();
				foreach (var spec in specs)
				{
					uow.Repository<Specifications>().Delete(spec);
				}

				// Xóa chính thiết bị
				uow.Repository<Equipment>().Delete(equipment);

				uow.SaveChanges();
				return Json("success", JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json("error", JsonRequestBehavior.AllowGet);
			}
		}

		
		//4.8.2025 thêm Danh sách tem
		[ValidateInput(false)]
		public ActionResult StampViewPartial(string selectedUserId, string selectedStampType, string selectedStatus)
		{
			var currentUser = userManager.FindByName(User.Identity.Name);
			bool isAdmin = User.IsInRole("Admin");
			bool isDeptDirector = User.IsInRole("DeptDirector");

			ViewBag.SelectedUserId = selectedUserId;
			ViewBag.SelectedStampType = selectedStampType;
			ViewBag.SelectedStatus = selectedStatus;
			// Lấy danh sách nhân viên
			if (isAdmin)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();

				userList.Insert(0, new SelectListItem { Value = "", Text = "KĐV: Toàn bộ" });
				ViewBag.UserList = userList;
			}

			if (isDeptDirector)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled && x.Department.Id == currentUser.Department.Id)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})				
				.ToList();

				userList.Insert(0, new SelectListItem { Value = "", Text = "Toàn Đơn vị" });
				ViewBag.UserList = userList;
			}

			if (!isAdmin && !isDeptDirector)
			{
				var userList = uow.Repository<AppUser>()
				.FindBy(x => x.TwoFactorEnabled && x.Id == currentUser.Id)
				.Select(x => new SelectListItem
				{
					Value = x.Id.ToString(),
					Text = x.DisplayName
				})
				.ToList();				
				ViewBag.UserList = userList;
			}

			var data = EquipmentDataProvider.GetStampSerialViewByUser(currentUser.Id, currentUser.Department.Id, isAdmin, isDeptDirector);
			//data = data.OrderBy(x => x.SerialNumber).ToList();
			// Filter
			if (!string.IsNullOrEmpty(selectedUserId))
			{
				int filterUserId;
				if (int.TryParse(selectedUserId, out filterUserId) && filterUserId > 0)
					data = data.Where(x => x.OwnerId == filterUserId).ToList();
			}

			if (!string.IsNullOrEmpty(selectedStampType))
			{
				byte filterStampType;
				if (byte.TryParse(selectedStampType, out filterStampType) && filterStampType > 0)
					data = data.Where(x => x.StampTypeId == filterStampType).ToList();
			}

			if (selectedStatus == "Tem chưa SD")
				data = data.Where(x => x.EquipName == selectedStatus).ToList();

			if (selectedStatus == "Tem đã SD")
				data = data.Where(x => x.EquipName != "Tem chưa SD").ToList();

			return PartialView("_StampViewPartial", data);
		}		

	}
}


