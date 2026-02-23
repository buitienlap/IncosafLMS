using IncosafCMS.Core.DomainModels;
using IncosafCMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncosafCMS.Web
{
    public class EquipmentAddedList
    {
        //public static List<Equipment> EquipmentsAdded
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["EquipmentsAdded"] == null)
        //            HttpContext.Current.Session["EquipmentsAdded"] = new List<Equipment>();

        //        return (List<Equipment>)HttpContext.Current.Session["EquipmentsAdded"];
        //    }
        //    set { HttpContext.Current.Session["EquipmentsAdded"] = value; }
        //}
    }
    public class AccTaskList
    {
        //public static List<AccTask> GetAccTasks
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["AccTasks"] == null)
        //            HttpContext.Current.Session["AccTasks"] = new List<AccTask>();

        //        return (List<AccTask>)HttpContext.Current.Session["AccTasks"];
        //    }
        //    set { HttpContext.Current.Session["AccTasks"] = value; }
        //}

        
        //public static void AddAccTask(AccTask accTask)
        //{
        //    List<AccTask> list = GetAccTasks;
        //    var listExitID = list.Select(x => x.Id).ToList();

        //    int id;
        //    do
        //    {
        //        id = RandomNumber();
        //    } while (listExitID.Contains(id));

        //    accTask.Id = id;

        //    //10.2.2026 ⭐ QUAN TRỌNG: Chuẩn hóa VAT + UnitPrice trước khi lưu
        //    NormalizeVATAndPrice(accTask);

        //    list.Add(accTask);
        //}
        
        //public static void UpdateAccTask(AccTask accTaskInfo)
        //{
        //    AccTask editedAccTask = GetAccTasks.First(m => m.Id == accTaskInfo.Id);

        //    editedAccTask.Name = accTaskInfo.Name;
        //    editedAccTask.Unit = accTaskInfo.Unit;
        //    editedAccTask.Amount = accTaskInfo.Amount;
        //    editedAccTask.vatType = accTaskInfo.vatType;
        //    editedAccTask.VAT = accTaskInfo.VAT;
        //    editedAccTask.AccTaskGroup = accTaskInfo.AccTaskGroup;
        //    editedAccTask.AccTaskNote = accTaskInfo.AccTaskNote;
        //    editedAccTask.Tyle1 = accTaskInfo.Tyle1;
        //    editedAccTask.Tyle2 = accTaskInfo.Tyle2;
        //    editedAccTask.Tyle3 = accTaskInfo.Tyle3;
        //    editedAccTask.Tyle4 = accTaskInfo.Tyle4;

        //    //10.2.2026  Gán tạm UnitPrice (sẽ chuẩn hóa lại)
        //    editedAccTask.UnitPrice = accTaskInfo.UnitPrice;

        //    //10.2.2026  ⭐ QUAN TRỌNG: Chuẩn hóa lại VAT + UnitPrice
        //    NormalizeVATAndPrice(editedAccTask);
        //}

        //public static void DeleteAccTask(int AccTaskId)
        //{
        //    List<AccTask> AccTasks = GetAccTasks;
        //    AccTasks.Remove(AccTasks.Where(m => m.Id == AccTaskId).First());
        //}
        private static int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1, int.MaxValue);
        }

        //Hàm dưới đây để xử lý khi Save UnitPrice với các trường hợp thuế VAT khác nhau
        //private static void NormalizeVATAndPrice(AccTask task)
        //{
        //    if (task.vatType == VATType.khongthue || task.VAT <= 0)
        //    {
        //        task.VAT = 0;
        //        // UnitPrice giữ nguyên
        //    }
        //    else
        //    {
        //        //10.2.2026  UnitPrice_noVAT là giá người dùng đang nhìn / nhập
        //        task.UnitPrice = Math.Round(
        //            task.UnitPrice / (1 + task.VAT / 100),2,MidpointRounding.AwayFromZero) * (1 + task.VAT / 100);

        //        task.UnitPrice = Math.Round(task.UnitPrice, 2, MidpointRounding.AwayFromZero);
        //    }
        //}
    }

    public class EquipmentSpecificationsList
    {
        //public static List<Specifications> GetEquipmentSpecifications
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["EquipmentSpecifications"] == null)
        //            HttpContext.Current.Session["EquipmentSpecifications"] = new List<Specifications>();

        //        return (List<Specifications>)HttpContext.Current.Session["EquipmentSpecifications"];
        //    }
        //    set { HttpContext.Current.Session["EquipmentSpecifications"] = value; }
        //}

        //public static void AddSpecifications(Specifications Specifications)
        //{
        //    List<Specifications> list = GetEquipmentSpecifications;
        //    var listExitID = list.Select(x => x.Id).ToList();
        //    int id;
        //    do
        //    {
        //        id = RandomNumber();
        //    } while (listExitID.Contains(id));
        //    Specifications.Id = id;

        //    list.Add(Specifications);
        //}

        //public static void UpdateSpecifications(Specifications SpecificationsInfo)
        //{
        //    Specifications editedSpecifications = GetEquipmentSpecifications.Where(m => m.Id == SpecificationsInfo.Id).First();

        //    editedSpecifications.Name = SpecificationsInfo.Name;
        //    editedSpecifications.Value = SpecificationsInfo.Value;
        //    editedSpecifications.f_unit = SpecificationsInfo.f_unit; //Hưng Thêm 08.05.2025            
        //    editedSpecifications.f_key = SpecificationsInfo.f_key;
        //}

        //public static void DeleteSpecifications(int SpecificationsId)
        //{
        //    List<Specifications> Specificationss = GetEquipmentSpecifications;
        //    Specificationss.Remove(Specificationss.Where(m => m.Id == SpecificationsId).First());
        //}
        private static int RandomNumber()
        {
            Random random = new Random();
            return random.Next(1, int.MaxValue);
        }
    }

    
    
}