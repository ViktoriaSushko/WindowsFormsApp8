using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp8
{
    class FileComparer : IEqualityComparer<FileInfo>
    {
        public bool Equals(FileInfo x, FileInfo y)
        {
            //имя, размер, дата обновления
            return x.Name == y.Name && x.Length == y.Length && x.LastWriteTime == y.LastWriteTime;
        }
        //возвращаем одинаковы результат
        public int GetHashCode(FileInfo obj)
        {
            return $"{obj.Name} {obj.Length} {obj.LastWriteTime}".GetHashCode();
        }
    }
}
