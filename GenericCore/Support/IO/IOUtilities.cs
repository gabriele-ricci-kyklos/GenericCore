using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support
{
    //credits: https://stackoverflow.com/a/50405099/4499267

    //TODO: do some tests
    public class IOUtilities
    {
        public static bool EmptyFolder(string pathName)
        {
            bool errors = false;
            DirectoryInfo dir = new DirectoryInfo(pathName);

            if(!dir.Exists)
            {
                return false;
            }

            foreach (FileInfo fi in dir.EnumerateFiles())
            {
                try
                {
                    fi.IsReadOnly = false;
                    fi.Delete();

                    //Wait for the item to disapear (avoid 'dir not empty' error).
                    while (fi.Exists)
                    {
                        System.Threading.Thread.Sleep(10);
                        fi.Refresh();
                    }
                }
                catch (Exception)
                {
                    errors = true;
                }
            }

            IList<DirectoryInfo> diList = new List<DirectoryInfo>();
            IList<Task<bool>> taskList = new List<Task<bool>>();

            foreach (DirectoryInfo di in dir.EnumerateDirectories())
            {
                taskList.Add(Task.Factory.StartNew(() => EmptyFolder(di.FullName)));
                diList.Add(di);
            }

            Task.WaitAll(taskList.ToArray());

            foreach (DirectoryInfo di in diList)
            {
                try
                {
                    di.Delete();

                    //Wait for the item to disapear (avoid 'dir not empty' error).
                    while (di.Exists)
                    {
                        System.Threading.Thread.Sleep(10);
                        di.Refresh();
                    }
                }
                catch (Exception)
                {
                    errors = true;
                }
            }

            if(taskList.Any(x => !x.Result))
            {
                errors = true;
            }

            return !errors;
        }

        public static bool DeleteFolder(string pathName)
        {
            if(!EmptyFolder(pathName))
            {
                return false;
            }

            try
            {
                Directory.Delete(pathName, true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
