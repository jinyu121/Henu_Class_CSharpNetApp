using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace A_4
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“WebFileService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 WebFileService.svc 或 WebFileService.svc.cs，然后开始调试。
    public class WebFileService : IWebFileService
    {
        public WebFile[] GetFileList() {
            List<WebFile> fileList = new List<WebFile>();
            string[] files = Directory.GetFiles(@"C:\windows");
            foreach (string f in files){
                WebFile ff = new WebFile();
                FileInfo fi = new FileInfo(f);
                ff.fileName = fi.Name;
                ff.filePath = f;
                ff.fileSize = fi.Length;
                fileList.Add(ff);
            }
            return fileList.ToArray();
        }
        public Stream GetFile(String file)
        {
            return File.OpenRead(file);
        }
    }
}
