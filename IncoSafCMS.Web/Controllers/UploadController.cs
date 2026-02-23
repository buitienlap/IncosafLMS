using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace IncosafCMS.Web.Controllers
{
    public class UploadController : Controller
    {
        /*
          [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file, int equipmentId)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string saveFolder = Server.MapPath("~/UploadedImages");
                    if (!Directory.Exists(saveFolder))
                        Directory.CreateDirectory(saveFolder);

                    string fileName = equipmentId + ".jpeg";
                    string savedPath = Path.Combine(saveFolder, fileName);

                    using (var stream = file.InputStream)
                    using (var originalImage = System.Drawing.Image.FromStream(stream))
                    {
                        int maxWidth = 1024;
                        if (originalImage.Width > maxWidth)
                        {
                            int newHeight = (int)(originalImage.Height * (maxWidth / (double)originalImage.Width));
                            var newSize = new System.Drawing.Size(maxWidth, newHeight);
                            using (var resizedImage = new System.Drawing.Bitmap(originalImage, newSize))
                            {
                                resizedImage.Save(savedPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                        else
                        {
                            originalImage.Save(savedPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }

                    return Content("~/UploadedImages/" + fileName);
                }
                catch (Exception ex)
                {
                    return new HttpStatusCodeResult(500, "Lỗi khi xử lý ảnh: " + ex.Message);
                }
            }

            return new HttpStatusCodeResult(400, "Không có file");
        }
         */

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase file, int equipmentId)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var folderPath = Server.MapPath("~/UploadedImages");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    var fileName = equipmentId + ".jpg"; // hoặc .png nếu muốn
                    var filePath = Path.Combine(folderPath, fileName);

                    // Resize ảnh nếu quá lớn
                    using (var img = System.Drawing.Image.FromStream(file.InputStream))
                    {
                        var resized = new Bitmap(img, new System.Drawing.Size(800, 600));
                        resized.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    string imagePath = $"~/UploadedImages/{fileName}";
                    return Json(new { success = true, imagePath = imagePath });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpGet]

        public JsonResult CheckImageExists(int equipmentId)
        {
            var folder = Server.MapPath("~/UploadedImages");
            var jpgPath = Path.Combine(folder, equipmentId + ".jpg");
            var pngPath = Path.Combine(folder, equipmentId + ".png");

            bool exists = System.IO.File.Exists(jpgPath) || System.IO.File.Exists(pngPath);
            return Json(new { exists = exists }, JsonRequestBehavior.AllowGet);
        }



    }
}
