var selectedcontract;
var selectedtaskofcontract;
var selectedaccridoftask;           // added by lapbt 10-mar-2025
var selectedaccridoftask_equipid;   // added by lapbt 05-jun-2025

// Begin TaskOfContract
function OnGridTaskOfContractFocusedRowChanged(s, e) {    
    selectedtaskofcontract = -1;
    s.GetRowValues(s.GetFocusedRowIndex(), 'Id;Name', OnGetRowTaskOfContractValues);
}

// Activity grid handlers (moved from Home/Index view)
window.OnActivityFocusedRowChanged = function (s, e) {
    LoadingPanel.Show();
    try {
        var rowIndex = s.GetFocusedRowIndex();
        if (rowIndex < 0) return;
        var key = s.GetRowKey(rowIndex);
        console.log('Activity id = ' + key);
        if (!key) return;
        $.get('/Activity/Details', { id: key }).done(function (html) {
            $('#activityDetailWrapper').html(html);
        }).fail(function () { toastr.error('Không tải được chi tiết hoạt động.'); });
    } catch (ex) {
        console.log(ex);
    }
    LoadingPanel.Hide();
};

window.OnActivityGridEndCallback = function (s, e) {
    try {
        var focus = s.GetFocusedRowIndex();
        if ((typeof focus !== 'number' || focus < 0) && s.GetVisibleRowsOnPage && s.GetVisibleRowsOnPage() > 0) {
            s.SetFocusedRowIndex(0);
            focus = 0;
        }
        var key = s.GetRowKey(focus);
        if (key) {
            $.get('/Activity/Details', { id: key }).done(function (html) {
                $('#activityDetailWrapper').html(html);
            });
        }
    } catch (ex) { console.log(ex); }
};

function OnGetRowTaskOfContractValues(values) {    
    if (values[0]) {
        selectedtaskofcontract = parseInt(values[0]);
        console.log("added by lapbt. Select task in gridview. selectedtaskofcontract = " + selectedtaskofcontract);

        if (selectedtaskofcontract && typeof selectedtaskofcontract === "number") {
            var createNewEquipmentItem = gvTaskOfContract.GetToolbar(0).GetItemByName('CreateNewEquipment');
            createNewEquipmentItem.SetEnabled(true);

            var editAccreditationItem = gvTaskOfContract.GetToolbar(0).GetItemByName('EditAccreditation');
            editAccreditationItem.SetEnabled(true);

            var printAccreditationItem = gvTaskOfContract.GetToolbar(0).GetItemByName('PrintAccreditation');
            printAccreditationItem.SetEnabled(true);

            // Load Accreditation list. Added by lapbt 10-mar-2025
            LoadAccreditationOfTask(false);
        }
        else {
            selectedtaskofcontract = -1;
            var createNewEquipmentItem1 = gvTaskOfContract.GetToolbar(0).GetItemByName('CreateNewEquipment');
            createNewEquipmentItem1.SetEnabled(false);

            var editAccreditationItem1 = gvTaskOfContract.GetToolbar(0).GetItemByName('EditAccreditation');
            editAccreditationItem1.SetEnabled(false);

            var printAccreditationItem1 = gvTaskOfContract.GetToolbar(0).GetItemByName('PrintAccreditation');
            printAccreditationItem1.SetEnabled(false);

            LoadAccreditationOfTask(true);      // load & disable
        }
    }
    else {

        var createNewEquipmentItem2 = gvTaskOfContract.GetToolbar(0).GetItemByName('CreateNewEquipment');
        createNewEquipmentItem2.SetEnabled(false);

        var editAccreditationItem2 = gvTaskOfContract.GetToolbar(0).GetItemByName('EditAccreditation');
        editAccreditationItem2.SetEnabled(false);

        var printAccreditationItem2 = gvTaskOfContract.GetToolbar(0).GetItemByName('PrintAccreditation');
        printAccreditationItem2.SetEnabled(false);

        LoadAccreditationOfTask(true);      // load & disable
    }
}

// sau khi load xong dữ liệu grid con
function OnGridTaskOfContractEndCallBack(s, e) {    
    if (needFocusFirstRow && s.GetVisibleRowsOnPage() > 0) {
        s.SetFocusedRowIndex(0); // focus dòng đầu tiên
    }
    needFocusFirstRow = false; // reset cờ, để user chọn dòng khác không bị override
    if (s.cpIsCustomCallback) {
        s.cpIsCustomCallback = false;
        OnGridTaskOfContractFocusedRowChanged(s, e);
    }
    else {
        var focusIndex = gvTaskOfContract.GetFocusedRowIndex();
        if (typeof focusIndex === "number" && focusIndex >= 0) {
            var createNewEquipmentItem = gvTaskOfContract.GetToolbar(0).GetItemByName('CreateNewEquipment');
            createNewEquipmentItem.SetEnabled(true);

            var editAccreditationItem = gvTaskOfContract.GetToolbar(0).GetItemByName('EditAccreditation');
            editAccreditationItem.SetEnabled(true);

            var printAccreditationItem = gvTaskOfContract.GetToolbar(0).GetItemByName('PrintAccreditation');
            printAccreditationItem.SetEnabled(true);
        }
        else {
            var createNewEquipmentItem1 = gvTaskOfContract.GetToolbar(0).GetItemByName('CreateNewEquipment');
            createNewEquipmentItem1.SetEnabled(false);

            var editAccreditationItem1 = gvTaskOfContract.GetToolbar(0).GetItemByName('EditAccreditation');
            editAccreditationItem1.SetEnabled(false);

            var printAccreditationItem1 = gvTaskOfContract.GetToolbar(0).GetItemByName('PrintAccreditation');
            printAccreditationItem1.SetEnabled(false);
        }
    }
}


function TaskOfContractMenuClick(s, e) {
    if (selectedcontract && typeof selectedcontract === "number" && selectedtaskofcontract && typeof selectedtaskofcontract === "number" && selectedtaskofcontract > 0) {
        if (e.item.name === "CreateNewEquipment") {
            $('#modalAddNewEquipmentWrapper').html("");
            $.ajax({
                url: '/Equipments/CreateFromLibViaContract/', // The method name + paramater
                data: { ContractID: selectedcontract, TaskID: selectedtaskofcontract },
                success: function (data) {
                    if (data) {
                        if (data === "overCount") {
                            swal({ title: "Quá số lượng thiết bị", text: "Vui lòng chỉnh sửa lại số lượng trong nội dung công việc.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        } else if (data === "false_1") {
                            swal({ title: "Không được chỉnh sửa", text: "Khi HĐ chưa cấp số và có ngày khởi tạo (CreateDate) > 60 ngày so với hiện hành.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        } else if (data === "false_2") {
                            swal({ title: "Không được chỉnh sửa", text: "Khi HĐ đã cấp số và có ngày khởi tạo (CreateDate) > 365 ngày so với hiện hành.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        } else if (data === "false_3") {
                            swal({ title: "Không được chỉnh sửa", text: "Khi HĐ đã kết thúc.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        } else if (data === "nullAccTaskNote") {
                            swal({ title: "Chưa gán loại hình công việc", text: "Yêu cầu vào chỉnh sửa HĐ, gán loại hình cv cho TB cần lấy số", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        }//Hưng thêm 6.3.2025 - Cảnh báo Chưa gán loại hình công việc vào đối tượng lấy số KQ
                        else if (data === "false") {
                            swal({ title: "Lỗi rồi 0:)", text: "Vui lòng tải lại trang để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        }
                        else {
                            $('#modalAddNewEquipmentWrapper').html(data);
                            $('#addnewEquipmentModal').modal('show');
                        }
                    }
                    else {
                        swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                },
                error: function (data) {
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            });
        }
        else if (e.item.name === "EditAccreditation") {
            //Cần xử lý: 
            //nếu đang chọn 1 TB ở cửa sổ Danh sách TB thì gọi luôn form sửa TB đó
            //Nếu Không chọn dòng TB nào thì show ra danh sách TB để lựa chọn edit như cách dưới
            $('#modalSelectEquipmentWrapper').html("");
            $.ajax({
                url: '/Equipments/SelectEquipmentForEditAccreditation/', // The method name + paramater
                data: { ContractID: selectedcontract, TaskID: selectedtaskofcontract },
                success: function (data) {
                    if (data) {
                        if (data !== "false") {
                            $('#modalSelectEquipmentWrapper').html(data);
                            $('#selectEquipmentModal').modal('show');
                        }
                        else {
                            swal({ title: "Có lỗi hoặc Không có quyền sửa hợp đồng đã cấp số!", text: "Vui lòng tải lại trang để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        }
                    }
                    else {
                        swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                },
                error: function (data) {
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            });
        }
        else if (e.item.name === "PrintAccreditation") {
            $('#modalSelectEquipmentForPrintWrapper').html("");
            $.ajax({
                url: '/Equipments/SelectEquipmentForPrintAccreditation/', // The method name + paramater
                data: { ContractID: selectedcontract, TaskID: selectedtaskofcontract },
                success: function (data) {
                    if (data) {
                        if (data !== "false") {
                            $('#modalSelectEquipmentForPrintWrapper').html(data);
                            $('#selectEquipmentForPrintModal').modal('show');
                        }
                        else {
                            swal({ title: "Lỗi rồi 5:)", text: "Form in GCN không phù hợp", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        }
                    }
                    else {
                        swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                },
                error: function (data) {
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            });
        }
    }
    else {
        if (e.item.name === "CreateNewEquipment" || e.item.name === "EditAccreditation")
            swal({ title: "Chưa chọn công việc!", text: "Vui lòng chọn công việc để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
    }
}




function OnSubmitCreateEquipment() {    
    var selectOriginalEquipment = cmbOriginalEquipment1.GetValue();
    var contractId = document.getElementsByName("txtContractIDForEditAccreditation")[0].value;
    if (selectOriginalEquipment && typeof selectOriginalEquipment === "string") {
        console.log("added by lapbt. Tao thiet bi -> Chon thiet bi: " + selectOriginalEquipment);

        swal({
            title: "Tạo mới thiết bị",
            text: "Vui lòng đợi đến khi hoàn tất.",
            type: "warning",
            showCancelButton: false,
            showConfirmButton: false
        });
        $.ajax(
            {
                type: "POST",
                data: { ContractID: contractId, TaskID: selectedtaskofcontract, originalEquipmentName: selectOriginalEquipment, originalEquipmentID: 0, Count: speCount.GetValue() },
                url: '/Equipments/CreateFromLibViaContract/',
                success: function (data) {
                    if (data) {
                        if (data[0] === "success") {
                            $('#addnewEquipmentModal').modal('hide');
                            $('#modalEditAccreditationWrapper').html("");
                            $.ajax({
                                url: '/Equipments/AccreditationViewViaContract/', // The method name + paramater
                                data: { id: data[1], contractId: contractId, numEquips: speCount.GetValue() },
                                success: function (data) {
                                    if (data) {
                                        $('#modalEditAccreditationWrapper').html(data);
                                        /*
                                         * Added by lapbt 22/08/2022
                                         * - Doi lai thu tu chay: (1) load ttin hdong xong -> (2) load ttin cv trong hdong
                                         * - Xly loi k0 load dc ds cv sau khi tao thiet bi -> k0 lay dc so BBKD
                                         *      - T.hop load ds bi trong se reload lai 3 lan, neu k0 dc se bao loi -> k0 he reload lai
                                         *      - Bo luon 2 khoi k0 thuc hien reload lai xem sao
                                         **/
                                        // (1)
                                        //var index = rdbGridContractFilter.GetSelectedIndex();
                                        //grvContracts.PerformCallback({ filtermode: parseInt(index) });

                                        // (2)
                                        //gvTaskOfContract.PerformCallback({
                                        //    selectedcontract: parseInt(selectedcontract)
                                        //});

                                        console.log("added by lapbt. OnSubmitCreateEquipment. selectedcontract = " + selectedcontract + ". selectedtaskofcontract = " + selectedtaskofcontract);
                                        
                                        $('#editAccreditationModal').modal('show');     // note by lapbt. form lay so BBKD
                                        swal({
                                            title: "Tạo mới thành công",
                                            text: "Quá trình hoàn tất.",
                                            type: "info",
                                            confirmButtonText: "Đồng ý",
                                            confirmButtonColor: "#23C6C8",
                                        });
                                    }
                                    else {
                                        swal({ title: "Lỗi rồi 1 :)", text: "Vui lòng tải lại trang để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                                    }
                                },
                                error: function (data) {
                                    swal({ title: "Lỗi rồi 2 :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                                }
                            });
                        }
                        else {
                            // added & edited by lapbt
                            var err_msg = data[1] ? data[1] : "";
                            swal({ title: "Lỗi rồi 3 :)", text: "Đã có lỗi xảy ra." + err_msg, type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        }
                    }
                    else {
                        swal({ title: "Lỗi rồi 4 :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                },
                error: function (data) {
                    swal({ title: "Lỗi rồi 5 :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            })
    }
    else {
        swal({ title: "Chưa chọn thiết bị!", text: "Vui lòng chọn thiết bị để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
    }
}

function OnSubmitSelectEquipment() {
    //var contractId = '@ViewData["ContractID"]';
    var contractId = document.getElementsByName("txtContractIDForEditAccreditation")[0].value;
    var taskId = document.getElementsByName("txtTaskIDForEditAccreditation")[0].value;
    
    
    var selectequipment = cmbSelectEquipment.GetValue();
    if (selectequipment && typeof selectequipment === "number") {

        console.log("popup sua bien ban. contractId = " + contractId + "; equipid = " + selectequipment);

        $('#selectEquipmentModal').modal('hide');

        $('#modalEditAccreditationWrapper').html("");
        $.ajax({
            type: "GET",
            url: '/Equipments/AccreditationViewViaContract/', // The method name + paramater
            data: { id: selectequipment, contractId: contractId/*, taskId: taskId*/ },
            success: function (data) {
                if (data) {
                    $('#modalEditAccreditationWrapper').html(data);
                    $('#editAccreditationModal').modal('show');
                }
                else {
                    swal({ title: "Lỗi rồi 2 :)", text: "Vui lòng tải lại trang để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            },
            error: function (data) {
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });
    }
    else {
        swal({ title: "Chưa chọn thiết bị!", text: "Vui lòng chọn thiết bị để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
    }
}

//function OnSubmitEditAccreditationViaContract() {
//    var dataToPost = $("#accreditationViewViaContractForm").serialize();
//    $.ajax(
//        {
//            type: "POST",
//            data: dataToPost,
//            url: '/Equipments/AccreditationViewViaContract/',
//            success: function (data) {
//                if (data === "success") {
//                    $('#editAccreditationModal').modal('hide');
//                    swal({ title: "Hoàn thành", text: "Đã chỉnh sửa thành công.", type: "success", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
//                }
//                else if (data === "required") {
//                    swal({ title: "Không được bỏ trống", text: "Chưa nhập các trường bắt buộc!", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
//                }
//                else if (data === "stamp_number_existed") {
//                    swal({ title: "Số tem kiểm định đã tồn tại", text: "Nhập lại một số tem kiểm định hợp lệ", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
//                }
//                else {
//                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
//                }
//            },
//            error: function (data) {
//                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
//            }
//        });
//}

function OnSubmitSelectEquipmentForPrint() {   

    var selectequipment = cmbSelectEquipmentForPrint.GetValue();
    if (selectequipment && typeof selectequipment === "number") {
        console.log("popup print equipment id = " + selectequipment);
        $('#selectEquipmentForPrintModal').modal('hide');       

        var urlAccreditationCertificateReportA5 = '/Reports/AccreditationCertificateReportA5/' + selectequipment;// '@Url.Action("AccreditationCertificateReportA5", "Reports", new { id = "_id_" })'.replace('_id_', selectequipment);
        var win = window.open(urlAccreditationCertificateReportA5, '_blank');
        win.focus();
    }
    else {
        swal({ title: "Chưa chọn thiết bị!", text: "Vui lòng chọn thiết bị để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
    }
}
// End TaskOfContract



// Begin AccreditionOfTask (xử lý sự kiện ở grid ds thiết bị)

function LoadAccreditationOfTask(disable) {
    console.log("added by lapbt. Load accreditation of task. selectedcontract = " + selectedcontract + ". selectedtaskofcontract = " + selectedtaskofcontract);

    // sdung phương thức `PerformCallback` của gridview để call controller gọi lấy DL từng grid.
    gvAccreditationOfTask.PerformCallback({
        selectedtaskofcontract: parseInt(selectedtaskofcontract)
    });

    // disable = true khi k0 có DL, sẽ disable các nút trên tool của grid
    // $("#btnContractReport").prop('disabled', disable);       // chưa có nút chức năng ở grid, nên đang đóng code
}

function OnGridAccreditationOfTaskFocusedRowChanged(s, e) {
    selectedaccridoftask = -1;
    selectedaccridoftask_equipid = -1;
    s.GetRowValues(s.GetFocusedRowIndex(), 'Id;equipmentName;equipmentId', OnGetRowAccreditationOfTaskValues);
}


function OnGetRowAccreditationOfTaskValues(values) {
    if (values[0]) {
        selectedaccridoftask = parseInt(values[0]);
        console.log("added by lapbt. Select accreditation in gridview. selectedaccridoftask = " + selectedaccridoftask + ". Respone html at ContractController.AccreditationOfTaskContractPartial");

        if (selectedaccridoftask && typeof selectedaccridoftask === "number") {
            selectedaccridoftask_equipid = parseInt(values[2]);     // khi thực sự chọn ở grid t.bi sẽ lấy equipmentId

            var printAccreditationItem = gvAccreditationOfTask.GetToolbar(0).GetItemByName('PrintAccreditationDirect');
            printAccreditationItem.SetEnabled(true);
        }
        else {
            selectedaccridoftask = -1;

            var printAccreditationItem = gvAccreditationOfTask.GetToolbar(0).GetItemByName('PrintAccreditationDirect');
            printAccreditationItem.SetEnabled(false);
        }
    }
    else {
        var printAccreditationItem = gvAccreditationOfTask.GetToolbar(0).GetItemByName('PrintAccreditationDirect');
        printAccreditationItem.SetEnabled(false);
    }
}

var deletedRowVisibleIndex = null;
function OnGridAccreditationOfTaskEndCallBack(s, e) {
    if (s.cpIsCustomCallback) {
        s.cpIsCustomCallback = false;
        OnGridAccreditationOfTaskFocusedRowChanged(s, e);
    }
    else {
        // --- Phần focus lại dòng tiếp theo sau khi xóa ---
        if (deletedRowVisibleIndex !== null) {
            var totalRows = s.GetVisibleRowsOnPage();
            var newIndex = (deletedRowVisibleIndex >= totalRows)
                ? totalRows - 1   // Nếu xóa dòng cuối, focus lùi lại 1
                : deletedRowVisibleIndex; // Focus dòng tiếp theo

            if (newIndex >= 0) {
                s.SetFocusedRowIndex(newIndex);
                // gọi lại để cập nhật selectedaccridoftask_equipid
                OnGridAccreditationOfTaskFocusedRowChanged(s, e);
            }
            deletedRowVisibleIndex = null;
        }
        // bật/tắt nút Print
        var focusIndex = gvAccreditationOfTask.GetFocusedRowIndex();
        if (typeof focusIndex === "number" && focusIndex >= 0) {
            var printAccreditationItem = gvAccreditationOfTask.GetToolbar(0).GetItemByName('PrintAccreditationDirect');
            printAccreditationItem.SetEnabled(true);
        }
        else {
            var printAccreditationItem = gvAccreditationOfTask.GetToolbar(0).GetItemByName('PrintAccreditationDirect');
            printAccreditationItem.SetEnabled(false);
        }
    }
}

function AccreditationOfTaskMenuClick(s, e) {
    if (selectedcontract && typeof selectedcontract === "number" &&
        selectedtaskofcontract && typeof selectedtaskofcontract === "number" && selectedtaskofcontract > 0 && 
        selectedaccridoftask && typeof selectedaccridoftask === "number" && selectedaccridoftask > 0 && 
        selectedaccridoftask_equipid && typeof selectedaccridoftask_equipid === "number" && selectedaccridoftask_equipid > 0) {
        if (e.item.name === "PrintAccreditationDirect") {
            console.log("direct print equipment id = " + selectedaccridoftask_equipid);
            // 
            var urlAccreditationCertificateReportA5 = '/Reports/AccreditationCertificateReportA5/' + selectedaccridoftask_equipid;
            var win = window.open(urlAccreditationCertificateReportA5, '_blank');
            win.focus();

        } else if (e.item.name === "EditAccreditationDirect") {
            console.log("direct sua bien ban. contractid = " + selectedcontract + "; equipid = " + selectedaccridoftask_equipid);

            $('#modalEditAccreditationWrapper').html("");
            $.ajax({
                type: "GET",
                url: '/Equipments/AccreditationViewViaContract/', // The method name + paramater
                data: { id: selectedaccridoftask_equipid, contractId: selectedcontract/*, taskId: taskId*/ },
                success: function (data) {
                    if (data) {
                        $('#modalEditAccreditationWrapper').html(data);
                        $('#editAccreditationModal').modal('show');
                    }
                    else {
                        swal({ title: "Lỗi rồi 2 :)", text: "Vui lòng tải lại trang để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                },
                error: function (data) {
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            });

        } else if (e.item.name === "DeleteEquipment") {
            swal({
                title: "Xác nhận xóa thiết bị",
                text: "Bạn có chắc chắn muốn xóa thiết bị này không?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#aaa",
                confirmButtonText: "Xóa",
                cancelButtonText: "Hủy"
            }, function (isConfirm) {
                if (isConfirm) {
                    // Lưu lại chỉ số dòng đang focus để set lại sau khi refresh
                    deletedRowVisibleIndex = gvAccreditationOfTask.GetFocusedRowIndex();
                    $.ajax({
                        url: '/Equipments/DeleteEquipmentCompletely',
                        method: 'POST',
                        data: { id: selectedaccridoftask_equipid },
                        success: function (res) {
                            if (res === 'success') {
                                toastr.success("Đã xóa thiết bị.");
                                // 🔹 Gọi luôn refresh grid
                                gvAccreditationOfTask.Refresh();                                
                            } else if (res === 'notDelete') {
                                toastr.warning("Thiết bị đã quá 5 ngày kể từ khi tạo, không thể xóa.");
                            } else if (res === 'notfound') {
                                toastr.error("Không tìm thấy thiết bị.");
                            } else {
                                toastr.error("Lỗi khi xóa thiết bị.");
                            }
                        }
                    });
                }
            });

        }

    }
    else {
        swal({ title: "Chưa chọn thiết bị!", text: "Vui lòng chọn thiết bị để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
    }
}


// End AccreditionOfTask


// --- Cleanup overrides: replace heavy/removed functions with lightweight stubs ---
// These override earlier definitions so we don't need to edit many places in the file.
(function () {
    // No-op equipment toolbar click
    window.EquipmentToolBarClick = function (s, e) { if (e) e.processOnServer = false; };

    // Disable bulk print on main page; keep function to avoid missing refs
    window.PrintSelected = function () { console.log('PrintSelected disabled'); };

    // Disable edit/show modals that were removed from the view
    window.showEditSpecificationsModal = function (equipmentId) { console.log('showEditSpecificationsModal disabled', equipmentId); };
    window.showEditAccreditationModal = function (equipmentId, contractId) { console.log('showEditAccreditationModal disabled', equipmentId, contractId); };

    // Disable upload/print helper
    window.uploadAndPrint = function (formData, equipmentId) { console.log('uploadAndPrint disabled', equipmentId); };

    // Load stamps via AJAX into placeholder created in Index.cshtml
    window.showStampsOfContract = function () {
        $.get('/Equipments/StampViewPartial')
            .done(function (data) {
                $('#stampGridPlaceholder').html(data);
                $('#stampModal').modal('show');
            })
            .fail(function () { toastr.error('Không tải được danh sách tem.'); });
    };

    // Replace dashboard/chart heavy functions with stubs to avoid runtime chart calls
    window.drawProductionChart = function () { console.log('drawProductionChart disabled'); };
    window.drawProductionChartLH = function () { console.log('drawProductionChartLH disabled'); };
    window.loadProductionChart = function () { console.log('loadProductionChart disabled'); };
    window.loadProductionChartLH = function () { console.log('loadProductionChartLH disabled'); };
    window.loadLoaiHinhPieChart = function () { console.log('loadLoaiHinhPieChart disabled'); };
    window.drawLoaiHinhPieChart = function () { console.log('drawLoaiHinhPieChart disabled'); };
    window.valueLabelPlugin = null;

    console.log('index.js cleanup overrides applied');
})();



function OnGridFocusedRowChanged(s, e) {
    LoadingPanel.Show();

    $("#btnContractReport").prop('disabled', true);
    $("#btnEditContract").prop('disabled', true);
    $("#btnDelete").prop('disabled', true);
    $("#btnpriceQuotationDetail").prop('disabled', true);

    selectedcontract = parseInt(s.GetRowKey(s.GetFocusedRowIndex()));   

    console.log("added by lapbt. current select contract id = " + selectedcontract);    

    if (selectedcontract && typeof selectedcontract === "number") {

        $.ajax({
            url: '/Contracts/GetContractById/',
            data: { id: selectedcontract },
            success: function (data) {
                if (data) {
                    LoadingPanel.Hide();
                    grvContracts.Focus();                    
                    
                    cContractName.textContent = data.Name ? data.Name : "N/A";
                    cName.textContent = data.CustomerName ? data.CustomerName : "N/A";
                    //cRepresentative.textContent = data.CustomerRepresentative ? data.CustomerRepresentative : "N/A";
                    cAddress.textContent = data.CustomerAddress ? data.CustomerAddress : "N/A";
                    //cPhone.textContent = data.CustomerPhone ? data.CustomerPhone : "N/A";
                    //cFax.textContent = data.CustomerFax ? data.CustomerFax : "N/A";
                    //cAccountNumber.textContent = data.CustomerAccountNumber ? data.CustomerAccountNumber : "N/A";
                    cTaxID.textContent = data.CustomerTaxID ? data.CustomerTaxID : "N/A";       // added by lapbt
                    switch (data.contractType) {
                        case 1:
                            cContractTypeID.textContent = "Hợp đồng kinh tế";
                            break;
                        case 2:
                            cContractTypeID.textContent = "Giấy đề nghị thường";
                            break;
                        case 3:
                            cContractTypeID.textContent = "Giấy đề nghị theo Đơn hàng";
                            break;
                        case 4:
                            cContractTypeID.textContent = "Giấy đề nghị theo Hợp đồng nguyên tắc";
                            break;
                        case 5:
                            cContractTypeID.textContent = "Hợp đồng nguyên tắc";
                            break;
                        default:
                            cContractTypeID.textContent = "N/A";
                    }
                    cOwnName.textContent = data.OwnerDisplayName ? data.OwnerDisplayName : "N/A";
                    cKDV1Name.textContent = data.KDV1DisplayName ? data.KDV1DisplayName : "N/A";
                    cKDV2Name.textContent = data.KDV2DisplayName ? data.KDV2DisplayName : "";                    
                    cSignDate.textContent = data.GetSignDate ? data.GetSignDate : "N/A";
                    cCongNo.textContent = data.CongNo ? data.CongNo.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") : "0.000";
                    IsGiayDeNghi_Home.SetChecked(data.IsGiayDeNghi);       // added by lapbt. Dua checkbox nay ra home, right side
                    textBox1.SetText(data.RatioOfCompany);
                    textBox2.SetText(data.RatioOfInternal);
                    cKhoanCongTy.textContent = (data.RatioOfCompany && data.ValueRoC) ? "% = " + data.ValueRoC.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") : "0.000";
                    cKhoanCaNhan.textContent = (data.RatioOfInternal && data.ValueRoI) ? "% = " + data.ValueRoI.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") : "0.000";
                    //cKhoanCongTy.textContent = data.RatioOfCompany ? data.RatioOfCompany : 0;
                    //cKhoanCaNhan.textContent = data.RatioOfInternal ? data.RatioOfInternal : 0;
                    labelKhoanCongty.textContent = data.RatioOfCompany; //Hưng thêm 14.2.2025
                    labelKhoanCanhan.textContent = data.RatioOfInternal; //Hưng thêm 14.2.2025

                    if (data.IsCompletedIPoC === true) {
                        $("#imgPaymentIPoCStatus").attr("src", "/Images/payment_completed.png");
                        $("#imgPaymentIPoCStatus").attr("title", "Hoàn thành thanh toán");
                    }
                    else {
                        $("#imgPaymentIPoCStatus").attr("src", "/Images/payment_uncompleted.png");
                        $("#imgPaymentIPoCStatus").attr("title", "Chưa hoàn thành thanh toán");
                    }
                    if (data.IsCompletedIPoI === true) {
                        $("#imgPaymentIPoIStatus").attr("src", "/Images/payment_completed.png");
                        $("#imgPaymentIPoIStatus").attr("title", "Hoàn thành thanh toán");
                    }
                    else {
                        $("#imgPaymentIPoIStatus").attr("src", "/Images/payment_uncompleted.png");
                        $("#imgPaymentIPoIStatus").attr("title", "Chưa hoàn thành thanh toán");
                    }

                    cValue.textContent = data.Value ? data.Value.toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") : "0.000";
                    //cValuetoWord.textContent = data.Value ? DocTienBangChu(data.Value) + " đồng." : "Không đồng";

                    // đặt lại các link vào item menu, để có thể tương tác đc với hợp đồng đang chọn

                    var url = '/Home/ContractDetails/' + selectedcontract; //'@Url.Action("ContractDetails", "Home", new { id = "_id_" })'.replace('_id_', selectedcontract);
                    contractDetailsLink.href = url;                    

                    var urlContractSuggestReport = '/Reports/ContractSuggestReport/' + selectedcontract;  //'@Url.Action("ContractSuggestReport", "Reports", new { id = "_id_" })'.replace('_id_', selectedcontract);
                    contractSuggestReportLink.href = urlContractSuggestReport;

                    var urlTurnOverSuggestReport = '/Reports/TurnOverSuggestReport/' + selectedcontract; //'@Url.Action("TurnOverSuggestReport", "Reports", new { id = "_id_" })'.replace('_id_', selectedcontract);
                    turnOverSuggestReportLink.href = urlTurnOverSuggestReport;

                    var urlContractReport = '/Reports/ContractReport/' + selectedcontract; //'@Url.Action("ContractReport", "Reports", new { id = "_id_" })'.replace('_id_', selectedcontract);
                    contractReportLink.href = urlContractReport;

                    // added by lapbt 19-jan-2025. Clone from ContractSuggestReport
                    var urlContractAcceptanceReport = '/Reports/ContractAcceptanceReport/' + selectedcontract; 
                    contractAcceptanceReportLink.href = urlContractAcceptanceReport;

                    LoadContractDetail(false); //Load danh sách công việc của hợp đồng
                    LoadContractInvoices(false); //Load danh sách xuất hóa đơn của hợp đồng
                    LoadContractPayment(false); //Load danh sách thu nợ của hợp đồng
                }
                else {
                    SetContractDetailDefault();
                    selectedcontract = -1;
                    LoadContractDetail(true);
                    LoadContractInvoices(true);
                    LoadContractPayment(true);

                    console.log("added by lapbt. not found contract. selectedcontract = -1");
                    swal({ title: "Không tìm thấy hợp đồng!", text: "Vui lòng tải lại trang.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            },
            error: function (data) {
                SetContractDetailDefault();
                selectedcontract = -1;
                LoadContractDetail(true);
                LoadContractInvoices(true);
                LoadContractPayment(true);

                console.log("added by lapbt. reload contract ajax error. selectedcontract = -1");
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });

    }
    else {
        console.log("added by lapbt. reload contract error. current select id is none. selectedcontract = -1");
        SetContractDetailDefault();
        selectedcontract = -1;
        LoadContractDetail(true);
        LoadContractInvoices(true);
        LoadContractPayment(true);
    }    
}
var needFocusFirstRow = false; // cờ kiểm soát
function LoadContractDetail(disable) {
    console.log("added by lapbt. LoadContractDetail. selectedcontract = " + selectedcontract);
    // sdung phương thức `PerformCallback` của gridview để call controller gọi lấy DL từng grid. 
    needFocusFirstRow = true; // khi load từ grid cha thì bật cờ
    gvTaskOfContract.PerformCallback({
        selectedcontract: parseInt(selectedcontract)
    });    

    $("#btnContractReport").prop('disabled', disable);
    $("#btnEditContract").prop('disabled', disable);
    $("#btnDelete").prop('disabled', disable);
    $("#btnpriceQuotationDetail").prop('disabled', disable);
}

//21.9.2025 thêm load danh sách hóa đơn
function LoadContractInvoices(disable) {
    console.log("LoadContractInvoices. selectedcontract = " + selectedcontract);

    DoanhThuGrid.PerformCallback({
        selectedcontract: parseInt(selectedcontract)
    });
}

//21.9.2025 thêm load danh sách thu nợ
function LoadContractPayment(disable) {
    console.log("LoadContractPayment. selectedcontract = " + selectedcontract);

    ThuNoGrid.PerformCallback({
        selectedcontract: parseInt(selectedcontract)
    });
}




function SetContractDetailDefault() {
    LoadingPanel.Hide();
    grvContracts.Focus();

    cContractName.textContent = "N/A";
    cName.textContent = "N/A";
    cRepresentative.textContent = "N/A";
    cAddress.textContent = "N/A";
    cPhone.textContent = "N/A";
    cFax.textContent = "N/A";
    cAccountNumber.textContent = "N/A";
    cOwnName.textContent = "N/A";
    cKDV1Name.textContent = "N/A";
    cKDV2Name.textContent = "";    
    cSignDate.textContent = "N/A";
    cCongNo.textContent = "0.000";
    textBox1.SetText(0);
    textBox2.SetText(0);
    cKhoanCongTy.textContent = "0";
    cKhoanCaNhan.textContent = "0";
}

function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return day + '/' + month + '/' + year;
}
function OnGridContractsEndCallBack(s, e) {
    if (s.cpIsCustomCallback) {
        s.cpIsCustomCallback = false;
        OnGridFocusedRowChanged(s, e);
    }   
    OnGridFocusedRowChanged(s, e);    
}

// Chọn trạng thái hợp đồng
/*
function OnRadioButtonSelectedIndexChanged(s, e) {
    var index = s.GetSelectedIndex();
    var ownerId = cmbNhanVien.GetValue();
    var departmentCode = cmbPhongBan.GetValue();
    var year = cmbFinancialYear.GetValue();

    var args = {
        filtermode: parseInt(index),
        departmentCode: departmentCode,
        year: year
    };
    if (ownerId) args.ownerId = ownerId;   // chỉ set khi có chọn nhân viên   

    grvContracts.PerformCallback(args);
}
*/
function OnRadioButtonSelectedIndexChanged(s, e) {
    var selectedIndex = s.GetSelectedIndex();
    var ownerId = cmbNhanVien.GetValue();
    var departmentCode = cmbPhongBan.GetValue();
    var year = cmbFinancialYear.GetValue();

    var args = {
        filtermode: selectedIndex,
        departmentCode: departmentCode,
        year: year
    };
    if (ownerId) args.ownerId = ownerId;

    // Nếu chọn 3–6 → mở panel + load grid 1 lần duy nhất
    if (selectedIndex >= 3 && selectedIndex <= 6) {
        var handled = expandListAndReload(args);
        if (handled) return;  // ⛔ không gọi grvContracts.PerformCallback nữa
    }

    // Trường hợp bình thường → load 1 lần
    grvContracts.PerformCallback(args);
}


function expandListAndReload(args) {
    var $detailCol = $("#detail-panel");
    var $listCol = $("#list-panel");

    // Chỉ chạy nếu đang thu hẹp
    if ($detailCol.is(":visible")) {

        // Mở panel danh sách
        $detailCol.hide();
        $listCol.removeClass("col-lg-8").addClass("col-lg-12");
        $("#btnToggleDetail").html('<i class="fa fa-angle-double-left"></i>')
            .attr("title", "Hiện chi tiết");

        $("#cmbPhongBanWrapper").show();
        $("#cmbTaiChinhWrapper").show();
        $("#cmbFinancialYearWrapper").show();

        // Lưu dòng focus
        var lastFocusIndex = -1;
        try {
            if (typeof grvContracts !== "undefined" && grvContracts)
                lastFocusIndex = grvContracts.GetFocusedRowIndex();
        } catch (ex) { }

        // Gọi toggle + callback filter trong 1 lần load
        args.toggle = true;  // đánh dấu gọi từ toggle

        $.post("/Contracts/ContractToggleColumns", args, function (html) {
            $("#contractGridContainer").html(html);

            if (lastFocusIndex >= 0) {
                grvContracts.SetFocusedRowIndex(lastFocusIndex);
                if (typeof OnGridFocusedRowChanged === "function")
                    OnGridFocusedRowChanged(grvContracts, null);
            }
        });

        return true; // đã xử lý và load xong → không cần callback lần 2
    }

    return false; // panel không thu hẹp → cho phép callback bình thường
}


//Chọn phòng ban
function OnDepartmentListSelectedIndexChanged(s, e) {
    var filtermode = rdbGridContractFilter.GetSelectedIndex();
    var ownerId = cmbNhanVien.GetValue();
    var departmentCode = s.GetValue();
    var year = cmbFinancialYear.GetValue();

    var args = {
        filtermode: parseInt(filtermode),
        departmentCode: departmentCode,
        year: year
    };
    if (ownerId) args.ownerId = ownerId;

    grvContracts.PerformCallback(args);
}

// chọn nhân viên
function OnNhanVienSelectedIndexChanged(s, e) {
    var ownerId = s.GetValue();
    var filtermode = rdbGridContractFilter.GetSelectedIndex();
    var departmentCode = cmbPhongBan.GetValue();
    var year = cmbFinancialYear.GetValue();

    var args = {
        filtermode: parseInt(filtermode),
        departmentCode: departmentCode,
        year: year
    };
    if (ownerId) args.ownerId = ownerId;

    grvContracts.PerformCallback(args);
}

// Chọn năm tính tổng tài chính
function OnFinancialYearChanged(s, e) {
    var year = s.GetValue();                            // năm được chọn
    var filtermode = rdbGridContractFilter.GetSelectedIndex();
    var ownerId = cmbNhanVien.GetValue();
    var departmentCode = cmbPhongBan.GetValue();

    var args = {
        filtermode: parseInt(filtermode),
        departmentCode: departmentCode,
        year: year                                       // ✅ thêm tham số năm
    };
    if (ownerId) args.ownerId = ownerId;

    grvContracts.PerformCallback(args);                  // gọi callback grid
}

function editContract() {
    $('#modalEditWrapper').html("");
    $('#modalAddNewWrapper').html("");
    $('#modalInvoiceCreateWrapper').html("");
    if (selectedcontract && typeof selectedcontract === "number") {
        $('#loading_panel_edit').show();
        $.ajax({
            url: '/Contracts/Edit/' + selectedcontract, // The method name + paramater
            success: function (data) {
                $('#modalEditWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
                $('#loading_panel_edit').hide();
            },
        });
    }
    else {
        swal({
            title: "Máy chủ chưa có phản hồi",
            text: "Vui lòng thực hiện lại!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Đồng ý",
            closeOnConfirm: true,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                $('#editModal').modal('hide');
            }
        });
    }
}
function addnewContract() {
    $('#modalAddNewWrapper').html("");
    $('#modalEditWrapper').html("");
    $('#modalInvoiceCreateWrapper').html("");
    $('#loading_panel_create').show();
    $.ajax({
        url: '/Contracts/Create/', // The method name + paramater
        success: function (data) {
            if (data) {
                $('#modalAddNewWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
                $('#loading_panel_create').hide();
            }
            else {
                $('#addnewModal').modal('hide');
                swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        },
        error: function (data) {
            $('#addnewModal').modal('hide');
            swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
        }
    });
}
function addnewFromOldContract() {
    $('#modalAddNewFromOldContractWrapper').html("");
    $.ajax({
        url: '/Contracts/CreateFromContract/', // The method name + paramater
        success: function (data) {
            if (data)
                $('#modalAddNewFromOldContractWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
            else {
                $('#addnewFromOldContractModal').modal('hide');
                swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        },
        error: function (data) {
            $('#addnewFromOldContractModal').modal('hide');
            swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
        }
    });
}

function turnoverandpaymentContract() {
    $('#modalTurnOverAndPaymentWrapper').html("");
    if (selectedcontract && typeof selectedcontract === "number") {
        $.ajax({
            url: '/Contracts/TurnOverAndPaymentView/' + selectedcontract, // The method name + paramater
            success: function (data) {
                if (data)
                    $('#modalTurnOverAndPaymentWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
                else {
                    $('#turnoverandpaymentModal').modal('hide');
                    swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            },
            error: function (data) {
                $('#turnoverandpaymentModal').modal('hide');
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });
    }
    else {
        swal({
            title: "Máy chủ chưa có phản hồi",
            text: "Vui lòng thực hiện lại!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Đồng ý",
            closeOnConfirm: true,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                $('#turnoverandpaymentModal').modal('hide');
            }
        });
    }
}

function editContractNumber() {
    $('#modalEditContractNumberWrapper').html("");
    if (selectedcontract && typeof selectedcontract === "number") {
        $.ajax({
            url: '/Contracts/EditContractNumber/' + selectedcontract, // The method name + paramater
            success: function (data) {
                if (data)
                    $('#modalEditContractNumberWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
                else {
                    $('#editcontractnumberModal').modal('hide');
                    swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            },
            error: function (data) {
                $('#editcontractnumberModal').modal('hide');
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });
    }
    else {
        swal({
            title: "Máy chủ chưa có phản hồi",
            text: "Vui lòng thực hiện lại!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Đồng ý",
            closeOnConfirm: true,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                $('#editcontractnumberModal').modal('hide');
            }
        });
    }
}

function deregisterContractNumber() {
    $('#modalDeregisterContractNumberWrapper').html("");
    if (selectedcontract && typeof selectedcontract === "number") {
        $.ajax({
            url: '/Contracts/DeregisterContractNumber/' + selectedcontract, // The method name + paramater
            success: function (data) {
                console.log(data);
                if (data)
                    $('#modalDeregisterContractNumberWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
                else {
                    $('#deregisterContractNumberModal').modal('hide');
                    swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            },
            error: function (data) {
                $('#deregisterContractNumberModal').modal('hide');
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });
    }
    else {
        swal({
            title: "Máy chủ chưa có phản hồi",
            text: "Vui lòng thực hiện lại!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Đồng ý",
            closeOnConfirm: true,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                $('#deregisterContractNumberModal').modal('hide');
            }
        });
    }
}

function priceQuotationDetail() {
    $('#modalPriceQuotationDetailWrapper').html("");
    if (selectedcontract && typeof selectedcontract === "number") {
        $.ajax({
            url: '/PriceQuotations/PriceQuotationDetailViewPartial/' + selectedcontract, // The method name + paramater
            success: function (data) {
                if (data[0] === "false") {
                    $('#pricequotationdetailModal').modal('hide');
                    swal({ title: "Không có báo giá!", text: "Không tìm thấy báo giá của hợp đồng.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
                else
                    $('#modalPriceQuotationDetailWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
            },
            error: function (data) {
                $('#pricequotationdetailModal').modal('hide');
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });
    }
    else {
        swal({
            title: "Máy chủ chưa có phản hồi",
            text: "Vui lòng thực hiện lại!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Đồng ý",
            closeOnConfirm: true,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                $('#pricequotationdetailModal').modal('hide');
            }
        });
    }
}

/*
function ApprovedMenuClick(s, e) {
    if (e.item.name === "SetContractNumber") {
        $('#modalEditContractNumberWrapper').html("");
        if (selectedcontract && typeof selectedcontract === "number") {
            $.ajax({
                url: '/Contracts/EditContractNumber/' + selectedcontract, // The method name + paramater
                success: function (data) {
                    if (data) {
                        $('#modalEditContractNumberWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
                        $('#editcontractnumberModal').modal('show');
                    }
                    else {
                        $('#editcontractnumberModal').modal('hide');
                        swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                },
                error: function (data) {
                    $('#editcontractnumberModal').modal('hide');
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            });
        }
        else {
            swal({
                title: "Máy chủ chưa có phản hồi",
                text: "Vui lòng thực hiện lại!",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Đồng ý",
                closeOnConfirm: true,
                closeOnCancel: false
            }, function (isConfirm) {
                if (isConfirm) {
                    $('#editcontractnumberModal').modal('hide');
                }
            });
        }
    } else {
        var approvedtype = e.item.name === "Waiting" ? 0 : 1;
        if (selectedcontract && typeof selectedcontract === "number") {
            $.ajax({
                url: '/Contracts/ApplyContractStatus/', // The method name + paramater
                data: { idContract: selectedcontract, approvedType: approvedtype },
                success: function (data) {
                    if (data[0] === "false") {
                        swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                    else {
                        var index = rdbGridContractFilter.GetSelectedIndex();
                        grvContracts.PerformCallback({ filtermode: parseInt(index) });
                    }
                },
                error: function (data) {
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            });
        }
        else {
            swal({
                title: "Máy chủ chưa có phản hồi",
                text: "Vui lòng thực hiện lại!",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Đồng ý",
                closeOnConfirm: true,
                closeOnCancel: false
            });
        }
    }
}
*/
function ApprovedMenuClick(s, e) {
    if (!selectedcontract) return;

    switch (e.item.name) {
        // ---- xử lý status ----
        case "Waiting":
        case "Approved":
            var approvedtype = e.item.name === "Waiting" ? 0 : 1;
            $.ajax({
                url: '/Contracts/ApplyContractStatus/',
                data: { idContract: selectedcontract, approvedType: approvedtype },
                success: function (data) {
                    if (data[0] === "false") {
                        swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error" });
                    } else {
                        var index = rdbGridContractFilter.GetSelectedIndex();
                        grvContracts.PerformCallback({ filtermode: parseInt(index) });
                    }
                },
                error: function () {
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error" });
                }
            });
            break;

        case "SetContractNumber":
            $.ajax({
                url: '/Contracts/EditContractNumber/' + selectedcontract, // The method name + paramater
                success: function (data) {
                    if (data) {
                        $('#modalEditContractNumberWrapper').html(data); // This should be an empty div where you can inject your new html (the partial view)
                        $('#editcontractnumberModal').modal('show');
                    }
                    else {
                        $('#editcontractnumberModal').modal('hide');
                        swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    }
                },
                error: function (data) {
                    $('#editcontractnumberModal').modal('hide');
                    swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            });
            break;

        // ---- xử lý document ----
        case "ViewDocument":
            $.get('/Contracts/ViewDocuments/' + selectedcontract, function (data) {
                try {
                    // Nếu server trả JSON (không có hồ sơ) thì parse và hiện thông báo
                    var parsed = typeof data === "string" ? JSON.parse(data) : data;
                    if (parsed && parsed.success === false) {
                        swal({
                            title: "Thông báo",
                            text: parsed.message || "Hợp đồng này chưa có hồ sơ nào.",
                            type: "warning"
                        });
                        return;
                    }
                } catch (ex) {
                    // data không phải JSON => tiếp tục như HTML
                }

                // Nếu tới đây nghĩa là data là HTML partial
                $('#modalContractDocumentsWrapper').html(data);
                $('#contractDocumentsModal').modal('show');
            })
                .fail(function () {
                    swal({
                        title: "Lỗi rồi :)",
                        text: "Không tải được hồ sơ.",
                        type: "error"
                    });
                });
            break;
        case "UploadDocument":
            $.get('/Contracts/UploadDocument/' + selectedcontract, function (data) {
                $('#modalContractUploadWrapper').html(data);
                $('#contractUploadModal').modal('show');
            });
            break;

    }
}
//Xem hồ sơ lưu khi bấm phải chuột và chọn "Xem hồ sơ"
function ViewDocuments(contractId) {  
    $.get('/Contracts/ViewDocuments/' + selectedcontract, function (data) {
        $('#modalContractDocumentsWrapper').html(data);
        $('#contractDocumentsModal').modal('show');
    });

}
//Xóa hồ sơ khi bấm nút xóa ở Danh sách hồ sơ
function deleteDoc(docId) {
    if (!confirm("Bạn có chắc chắn muốn xóa hồ sơ này không?")) return;

    $.ajax({
        url: '/Contracts/DeleteDocument',
        type: 'POST',
        data: { id: docId },
        success: function (resp) {
            if (resp.success) {
                toastr.success("Đã xóa hồ sơ.");
                // Reload lại danh sách hồ sơ sau khi xóa
                $("#docList").load("/Contracts/ViewDocuments/" + resp.contractId + " #docList > *");
                $("#docPreviewFrame").attr("src", ""); // clear preview nếu cần                
            } else {
                toastr.error(resp.message || "Không thể xóa hồ sơ.");
            }
            
        },
        error: function () {
            toastr.error("Có lỗi trong quá trình xóa.");
        }
    });    
}


function ShowUploadPopup(contractId) {
    // mở popup upload file
    $('#uploadModal').data('id', contractId).modal('show');
    $('#contractViewModal').on('hidden.bs.modal', function () {
        // Reload lại grid chính khi modal ViewDocument đóng
        if (typeof gvContract !== 'undefined') {
            gvContract.PerformCallback();
        }
    });
}

function onCloseViewDoc() {
    $('#viewDocModal').modal('hide');
    if (typeof gvContract !== 'undefined') {
        gvContract.PerformCallback(); // reload lại grid chính
    }
}





function OnGetAutoContractNumberClick() {
    if (selectedcontract && typeof selectedcontract === "number") {

        $.ajax({
            url: '/Contracts/GetAutoContractNumber/', // The method name + paramater
            data: { ContractID: selectedcontract },
            success: function (data) {
                if (data) {
                    txtMaHD.SetText(data);
                }
                else {
                    swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            },
            error: function (data) {
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });



        //swal({
        //    title: "Bạn có muốn lấy mã?",
        //    text: "Lưu ý: Hệ thống sẽ không tạo lại những mã đã lấy.",
        //    type: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#DD6B55",
        //    confirmButtonText: "Đồng ý",
        //    cancelButtonColor: "#23C6C8",
        //    cancelButtonText: "Hủy bỏ",
        //    closeOnConfirm: true,
        //    closeOnCancel: true
        //}, function (isConfirm) {
        //    if (isConfirm) {
        //        $.ajax({
        //            url: '/Contracts/GetAutoContractNumber/', // The method name + paramater
        //            data: { ContractID: selectedcontract },
        //            success: function (data) {
        //                if (data) {
        //                    txtMaHD.SetText(data);
        //                }
        //                else {
        //                    swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
        //                }
        //            },
        //            error: function (data) {
        //                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
        //            }
        //        });
        //    } else {
        //        swal({ title: "Đã hủy bỏ", text: "Đã hủy bỏ thao tác.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
        //    }
        //});
    }
    else {
        swal({
            title: "Máy chủ chưa có phản hồi",
            text: "Vui lòng thực hiện lại!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Đồng ý",
            closeOnConfirm: true,
            closeOnCancel: false
        });
    }
}

function OnSoKQDKChangedViaContract(s, e) {
    $.ajax({
        url: '/Equipments/CheckNumberAccreditationAvailable/', // The method name + paramater
        data: { NumberAccre: s.GetValue() },
        success: function (data) {
            if (data === "NotAvailable") {
                swal({ title: "Số KQKĐ này đã có!", text: "Vui lòng điền lại lại trang để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        }
    });
}

function OnSoHopDongChangedViaContract(s, e) {
}

function ContractToolBarClick(s, e) {
    if (e.item.name === "RefreshContractData") {
        $.ajax({
            url: '/Contracts/RefreshContractData/', // The method name + paramater
            success: function (data) {
                var index = rdbGridContractFilter.GetSelectedIndex();
                grvContracts.PerformCallback({ filtermode: parseInt(index) });
            },
            error: function (data) {
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });
    }
}

function OnSubmitEditContractNumber() {
    if (selectedcontract && typeof selectedcontract === "number") {
        var mahd = txtMaHD.GetValue();
        var ratiocompany = cboRatioOfCompany.GetText();
        var contracttypeId = cboContractTypeId.GetValue();

        console.log(mahd);
        if (mahd === "" || mahd === undefined || mahd === null) {
            swal({ title: "Hợp đồng chưa được cấp số!", text: "", type: "warning", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            return;
        }

        $.ajax({
            url: '/Contracts/CheckSoHDAvailable/', // The method name + paramater
            data: { soHd: mahd },
            success: function (data) {
                if (data === "NotAvailable") {
                    swal({ title: "Số hợp đồng này đã tồn tại!", text: "", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                    return;
                }
                else {
                    $.ajax({
                        type: "POST",
                        data: { idContract: selectedcontract, maHD: mahd, ratioCompany: ratiocompany, contractTypeId: contracttypeId },     // lapbt bo sung 2 bien phia sau
                        url: '/Contracts/EditContractNumber/', // The method name + paramater
                        success: function (data) {
                            if (data) {
                                if (data === "success") {
                                    var index = rdbGridContractFilter.GetSelectedIndex();
                                    grvContracts.PerformCallback({ filtermode: parseInt(index) });
                                } else {
                                    swal({ title: "Đã có lỗi xảy ra!", text: data, type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                                }
                            }
                            else {
                                swal({ title: "Bạn không có quyền!", text: "Bạn không có quyền thực hiện việc này.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                            }
                        },
                        error: function (data) {
                            swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                        }
                    });
                }
            }
        });
    }
}
function invoiceCreate() {
    $('#modalEditWrapper').html("");
    $('#modalAddNewWrapper').html("");
    $('#modalInvoiceCreateWrapper').html("");
    if (selectedcontract && typeof selectedcontract === "number") {
        $.ajax({
            url: '/Contracts/InvoiceCreate/' + selectedcontract,
            success: function (data) {
                if (data) {
                    $('#modalInvoiceCreateWrapper').html(data);
                }
                else {
                    $('#invoiceCreateModal').modal('hide');
                    swal({ title: "Bạn không có quyền!", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                }
            },
            error: function (data) {
                $('#invoiceCreateModal').modal('hide');
                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
            }
        });
    } else {
        swal({
            title: "Máy chủ chưa có phản hồi",
            text: "Vui lòng thực hiện lại!",
            type: "warning",
            showCancelButton: false,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Đồng ý",
            closeOnConfirm: true,
            closeOnCancel: false
        });
    }
}

//function OnCreateInvoiceReportSubmit() {
//    return swal({
//        title: "Bạn có muốn xóa?",
//        text: "Thao tác này sẽ không thể khôi phục lại",
//        type: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#DD6B55",
//        confirmButtonText: "Đồng ý",
//        cancelButtonColor: "#23C6C8",
//        cancelButtonText: "Hủy bỏ",
//        closeOnConfirm: false,
//        closeOnCancel: false
//    }, function (isConfirm) {
//        if (isConfirm) {
//            OnCreateInvoiceReport();
//            return true;
//        } else {
//            swal({ title: "Đã hủy bỏ", text: "Đã hủy bỏ thao tác.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
//            return false;
//        }

//    });
//}

function OnCreateInvoiceReport() {

    var hdnb = HDNumber.GetValue();
    if (hdnb) {
        $.ajax({
            url: '/Contracts/CheckHDNumberValid/', // The method name + paramater
            data: { ContractID: selectedcontract, HDNumber: hdnb },
            success: function (data) {
                if (data === "NotValid") {
                    swal({
                        title: "Số hóa đơn này đã có!",
                        text: "Vui lòng điền lại để tiếp tục.",
                        type: "error",
                        showCancelButton: false,
                        confirmButtonColor: "#23C6C8",
                        confirmButtonText: "Đồng ý",
                        closeOnConfirm: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            HDNumber.Focus();
                        }
                    });
                }
                else if (data === "Valid") {
                    $.ajax({
                        url: '/Contracts/CheckHDValue/',
                        data: { ContractID: selectedcontract },
                        success: function (data) {
                            if (data === "over") {
                                swal({
                                    title: "Tổng hóa đơn lớn giá trị HĐ!",
                                    text: "Bạn có muốn tiếp tục in hóa đơn.",
                                    type: "error",
                                    showCancelButton: true,
                                    confirmButtonColor: "#DD6B55",
                                    confirmButtonText: "Đồng ý",
                                    cancelButtonColor: "#23C6C8",
                                    cancelButtonText: "Hủy bỏ",
                                    closeOnConfirm: true
                                }, function (isConfirm) {
                                    if (isConfirm) {
                                        $.ajax({
                                            type: "POST",
                                            url: '/Contracts/InvoiceCreate/',
                                            data: $('#invoiceCreateForm').serialize(),
                                            success: function (data) {
                                                $('#invoiceCreateModal').modal('hide');
                                                var url = '@Url.Action("InvoiceReport", "Contracts")';
                                                window.open(url)
                                            },
                                            error: function (data) {
                                                $('#invoiceCreateModal').modal('hide');
                                                swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                                            }
                                        });
                                    }
                                });
                            }
                            else {
                                $.ajax({
                                    type: "POST",
                                    url: '/Contracts/InvoiceCreate/',
                                    data: $('#invoiceCreateForm').serialize(),
                                    success: function (data) {
                                        $('#invoiceCreateModal').modal('hide');
                                        var url = '@Url.Action("InvoiceReport", "Contracts")';
                                        window.open(url)
                                    },
                                    error: function (data) {
                                        $('#invoiceCreateModal').modal('hide');
                                        swal({ title: "Lỗi rồi :)", text: "Đã có lỗi xảy ra.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
                                    }
                                });
                            }
                        }
                    });
                }
            }
        });
    }
    else {
        swal({ title: "Số hóa đơn đang bỏ trống", text: "Vui lòng nhập số hóa đơn để tiếp tục.", type: "error", confirmButtonColor: "#23C6C8", confirmButtonText: "Đồng ý" });
    }
}

function OnCheckHDNumberValid(s, e) {
    $.ajax({
        url: '/Contracts/CheckHDNumberValid/', // The method name + paramater
        data: { ContractID: selectedcontract, HDNumber: s.GetValue() },
        success: function (data) {
            if (data === "NotValid") {
                swal({
                    title: "Số hóa đơn này đã có!",
                    text: "Vui lòng điền lại để tiếp tục.",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonColor: "#23C6C8",
                    confirmButtonText: "Đồng ý",
                    closeOnConfirm: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        s.Focus();
                    } 
                });
            }
        }
    });
}

function OnBtnApDungKhoanCTClicked(s, e) {
    $.ajax({
        url: '/Contracts/ChangeTiLeKhoanCT/', // The method name + paramater
        data: { contractId: selectedcontract, tilekhoanCT: textBox1.GetValue() },
        success: function (data) {
            if (data === "NotValid") {
                swal({
                    title: "Cập nhật lỗi",
                    text: "Không có quyền cập nhật khi đã cấp số (Admin/Accountant). Hoặc tỷ lệ cần trong khoảng 50 đến 99.",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonColor: "#23C6C8",
                    confirmButtonText: "Đồng ý",
                    closeOnConfirm: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        s.Focus();
                    } 
                });
            }
        }
    });
}

function OnBtnApDungKhoanCNClicked(s, e) {
    $.ajax({
        url: '/Contracts/ChangeTiLeKhoanCN/', // The method name + paramater
        data: { contractId: selectedcontract, tilekhoanCN: textBox2.GetValue() },
        success: function (data) {
            if (data === "NotValid") {
                swal({
                    title: "Dữ liệu không đúng định dạng!",
                    text: "Vui lòng nhập số trong khoảng 0 đến 100.",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonColor: "#23C6C8",
                    confirmButtonText: "Đồng ý",
                    closeOnConfirm: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        s.Focus();
                    }
                });
            }
        }
    });
}

// added by lapbt. 23-jan-2024. Ghi lai thay doi Giaydenghi o home, chi tiet ben phai.
function OnChangeIsGiayDeNghiClicked(s, e) {
    $.ajax({
        url: '/Contracts/ChangeIsGiayDeNghi/', // The method name + paramater
        data: { contractId: selectedcontract, contractIsGiayDeNghi: IsGiayDeNghi_Home.GetChecked() },
        success: function (data) {
            if (data === "NotValid") {
                swal({
                    title: "Từ chối cập nhật!",
                    text: "Hợp đồng đã cấp số. Không đủ quyền sửa (Admin)",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonColor: "#23C6C8",
                    confirmButtonText: "Đồng ý",
                    closeOnConfirm: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        s.Focus();
                    }
                });
            }
        }
    });
}



