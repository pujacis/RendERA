using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RendERA.Models;
using RendERA.ServiceManager.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RendERA.Common
{
    public class Files : ControllerBase
    {
        
        private Dictionary<string, string> GetMimeTypes()
        {
            //allow document extebstions
            return new Dictionary<string, string>
            {
               
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".zip", "file/zip"},
                {".csv", "text/csv"},
                {".wmv", "video/x-ms-wmv"},
                {".avi", "video/x-msvideo"},
                {".mov", "video/quicktime"},
                {".3gp", "video/3gpp"},
                {".ts", "video/MP2T"},
                {".m3u8", "application/x-mpegURL"},
                {".mp4", "video/mp4"},
                {".123", "application/vnd.lotus-1-2-3"},
                {".3dmf", "x-world/x-3dmf"},
                {".3ds", "image/x-3ds"},
                {".669", "audio/x-mod"},
                {".ai", "application/postscript"},
                {".aif", "audio/aiff"},
                {".aiff", "audio/aiff"},
                {".ani", "application/x-navi-animation"},
                {".aos", "application/x-nokia-9000-communicator-add-on-software"},
                {".aps", "application/mime"},
                {".arc", "application/octet-stream"},
                {".arj", "application/arj"},
                {".art", "image/x-jg"},
                {".asf", "video/x-ms-asf"},
                {".asm", "text/x-asm"},
                {".asp", "text/asp"},
                {".asx", "video/x-ms-asf"},
                {".au", "audio/basic"},
                {".bin", "application/mac-binary"},
                {".bm", "image/bmp"},
                {".boo", "application/book"},
                {".book", "application/book"},
                {".c", "text/x-c"},
                {".c++", "text/plain"},
                {".ccad", "application/book"},
                {".class", "application/java"},
                {".com", "text/plain"},
                {".conf", "text/plain"},
                {".cpp", "text/x-c"},
                {".cpt", "application/x-cpt"},
                {".css", "text/css"},
                {".dcr", "application/x-director"},
                {".def", "text/plain"},
                {".dif", "video/x-dv"},
                {".dir", "application/x-director"},
                {".dl", "video/dl"},
                {".dot", "application/msword"},
                {".drw", "application/drafting"},
                {".dvi", "application/x-dvi"},
                {".dwg", "application/acad"},
                {".dxf", "application/dxf"},
                {".dxr", "application/x-director"},
                {".exe", "application/octet-stream"},
                {".gz", "application/x-gzip"},
                {".gzip", "application/x-gzip"},
                {".h", "text/plain"},
                {".hlp", "application/hlp"},
                {".htc", "text/x-component"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".htmls", "text/html"},
                {".htt", "text/webviewhtml"},
                {".ice", "x-conference/x-cooltalk"},
                {".ico", "image/x-icon"},
                {".inf", "application/inf"},
                {".jam", "audio/x-jam"},
                {".jav", "text/plain"},
                {".java", "text/plain"},
                {".jcm", "application/x-java-commerce"},
                {".jfif", "image/jpeg"},
                {".jfif-tbnl", "image/jpeg"},
                {".jpe", "image/jpeg"},
                {".jps", "image/x-jps"},
                {".js", "application/javascript"},
                {".latex", "application/x-latex"},
                {".lha", "application/lha"},
                {".lhx", "application/octet-stream"},
                {".list", "text/plain"},
                {".lsp", "application/x-lisp"},
                {".lst", "text/plain"},
                {".lzh", "application/x-lzh"},
                {".lzx", "application/lzx"},
                {".m3u", "audio/x-mpequrl"},
                {".man", "application/x-troff-man"},
                {".mid", "audio/midi"},
                {".mod", "audio/x-mod"},
                {".movie", "video/x-sgi-movie"},
                {".mp2", "audio/mpeg"},
                {".mp3", "video/mpeg"},
                {".mpa", "video/mpeg"},
                {".mpeg", "video/mpeg"},
                {".mpg", "video/mpeg"},
                {".mpga", "audio/mpeg"},
                {".pas", "text/pascal"},
                {".pcl", "application/x-pcl"},
                {".pct", "image/x-pict"},
                {".pcx", "image/x-pcx"},
                {".pic", "image/pict"},
                {".pict", "image/pict"},
                {".pl", "text/plain"},
                {".pm", "image/x-xpixmap"},
                {".pm4", "application/x-pagemaker"},
                {".pm5", "application/x-pagemaker"},
                {".pot", "application/mspowerpoint"},
                {".ppa", "application/vnd.ms-powerpoint"},
                {".pps", "application/mspowerpoint"},
                {".ppt", "application/mspowerpoint"},
                {".ppz", "application/mspowerpoint"},
                {".ps", "application/postscript"},
                {".psd", "application/octet-stream"},
                {".pwz", "application/vnd.ms-powerpoint"},
                {".py", "text/x-script.phyton"},
                {".pyc", "applicaiton/x-bytecode.python"},
                {".qt", "video/quicktime"},
                {".qtif", "image/x-quicktime"},
                {".ra", "audio/x-pn-realaudio"},
                {".ram", "audio/x-pn-realaudio"},
                {".rm", "application/vnd.rn-realmedia"},
                {".rpm", "audio/x-pn-realaudio-plugin"},
                {".rtf", "application/rtf"},
                {".rtx", "application/rtf"},
                {".rv", "video/vnd.rn-realvideo"},
                {".sgml", "text/sgml"},
                {".sh", "application/x-bsh"},
                {".shtml", "text/html"},
                {".ssi", "text/x-server-parsed-html"},
                {".tar", "application/x-tar"},
                {".tcl", "application/x-tcl"},
                {".text", "text/plain"},
                {".tgz", "application/gnutar"},
                {".tif", "image/tiff"},
                {".tiff", "image/tiff"},
                {".uri", "text/uri-list"},
                {".vcd", "application/x-cdlink"},
                {".vmd", "application/vocaltec-media-desc"},
                {".vrml", "model/vrml"},
                {".vsd", "application/x-visio"},
                {".vst", "application/x-visio"},
                {".vsw", "application/x-visio"},
                {".wav", "audio/wav"},
                {".wmf", "windows/metafile"},
                {".xla", "application/excel"},
                {".xlb", "application/excel"},
                {".xlc", "application/excel"},
                {".xld", "application/excel"},
                {".xlk", "application/excel"},
                {".xll", "application/excel"},
                {".xlm", "application/excel"},
                {".xm", "audio/xm"},
                {".xml", "text/xml"},
                {".z", "application/x-compress"},
                {".zoo", "application/octet-stream"},
                {".zsh", "text/x-scriptzsh"},
                {".xlt", "application/excel"},
                {".xlv", "application/excel"},
                {".xlw", "application/excel"},
                 {".txt", "text/plain"},
                 {".flac", "audio/flac"}
            };
        }

        private string GetContentType(string path)
        {
            //get uploaded document extention
            try
            {
                var types = GetMimeTypes();
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return types[ext];
            }
            catch(Exception ex)
            {
                return "Failed";
            }
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                //genrate unique string
                var bytes = new byte[40];
                using (var crypto = new RNGCryptoServiceProvider())
                    crypto.GetBytes(bytes);
                var base64 = Convert.ToBase64String(bytes);
                var result = Guid.NewGuid().ToString("N") + base64;
                result = Regex.Replace(result, "[^A-Za-z0-9]", "");

                //set uploaded documnet path
                var dirPath = "wwwroot\\Upload\\" + result;
                var path = Directory.GetCurrentDirectory();
                Directory.CreateDirectory(dirPath);
                path = Path.Combine(
                      Directory.GetCurrentDirectory(), dirPath,
                      file.GetFilename());
                dirPath = result + "\\" + file.GetFilename();

                //upload document
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    return dirPath;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<FileStreamResult> DownloadFile(string filename)
        {
            try
            {
                //Uploaded document path
                var path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\Upload", filename);

                //Download document
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var ContentType = GetContentType(path);
                if (ContentType == "Failed")
                {
                    return null;
                }
                return File(memory, ContentType, Path.GetFileName(path));
            }
            catch
            {
                return null;
            }
        }

        public void RemoveFile(string filename,string path) {
            try
            {
                var fullpath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                   "wwwroot\\Upload", path+"\\"+filename);


                if ((System.IO.File.Exists(fullpath)))
                {

                    System.IO.File.Delete(fullpath);
                }
                if ((Directory.Exists(path)))
                {
                    Directory.Delete(path);
                }
            }
            catch(Exception ex)
            {

            }
        }


    }
}
