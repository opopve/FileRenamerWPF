using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Tools;
using System.Linq;

namespace FileRenamerWPF
{
    public class FileSystemNode : PropertyChangedBase
    {
        public FileSystemNode(FileSystemInfo fileSystemObj) {
            if(!fileSystemObj.Exists)
                return;

            FileSystemObj = fileSystemObj;
            if(fileSystemObj is DirectoryInfo dir) {
                IsFile = false;
                FileExtension = string.Empty;
                dir.GetDirectories().OrderBy(o => o.Name).ToList().ForEach(s => ObservableChildren.Add(new FileSystemNode(s)));
                dir.GetFiles().OrderBy(o => o.Name).ToList().ForEach(s => ObservableChildren.Add(new FileSystemNode(s)));
                Path = dir.Parent.FullName;
            } else if(fileSystemObj is FileInfo file) {
                IsFile = true;
                FileExtension = file.Extension;
                Path = file.DirectoryName;
            }
            Name = FileSystemObj.Name;
        }

        bool isExpanded;
        [Display(Name = "Раскрыта папка")]
        public bool IsExpanded
        {
            get => isExpanded;
            set => SetField(ref isExpanded, value);
        }

        FileSystemInfo fileSystemObj;
        [Display(Name = "Объект файловой системы")]
        public FileSystemInfo FileSystemObj
        {
            get => fileSystemObj;
            set => SetField(ref fileSystemObj, value);
        }

        string path;
        [Display(Name = "Путь к файлу или папке")]
        public string Path
        {
            get => path;
            set => SetField(ref path, value);
        }

        string name;
        [Display(Name = "Имя файла или папки")]
        public string Name
        {
            get => name;
            set => SetField(ref name, value);
        }

        string fileExtension;
        [Display(Name = "Расширение файла")]
        public string FileExtension
        {
            get => fileExtension;
            set => SetField(ref fileExtension, value);
        }

        bool isFile;
        [Display(Name = "Файл?")]
        public bool IsFile
        {
            get => isFile;
            set => SetField(ref isFile, value);
        }

        ObservableCollection<FileSystemNode> observableChildren = new ObservableCollection<FileSystemNode>();
        [Display(Name = "Содержимое папки")]
        public ObservableCollection<FileSystemNode> ObservableChildren
        {
            get => observableChildren;
            set => SetField(ref observableChildren, value);
        }
    }
}
