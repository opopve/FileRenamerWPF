using System.Collections.ObjectModel;
using System.IO;
using Tools;

namespace FileRenamerWPF
{
    public class MainWindowVM : PropertyChangedBase
    {
        public MainWindowVM(string path) {
            Tree.Add(
                new FileSystemNode(new DirectoryInfo(path)) {
                    IsExpanded = true
                }
            );
        }

        ObservableCollection<FileSystemNode> tree;
        public virtual ObservableCollection<FileSystemNode> Tree
        {
            get
            {
                if(tree == null)
                    tree = new ObservableCollection<FileSystemNode>();
                return tree;
            }

            set => SetField(ref tree, value);
        }
    }
}
