namespace FolderSize;

public static class DirectoryInfoExtensions
{
    public static long GetDirectorySize(this DirectoryInfo directoryInfo, bool recursive = true)
    {
        long startDirectorySize = 0;
        if (directoryInfo == null || !directoryInfo.Exists)
            return startDirectorySize; // Return 0 if Directory does not exist.

        // Add size of files in the Current Directory to main size.
        foreach (var fileInfo in directoryInfo.GetFiles())
            startDirectorySize += fileInfo.Length;

        // Loop on Subdirectories in the Current Directory and calculate their files' size.
        if (recursive)
        {
            foreach (var subDirectory in directoryInfo.GetDirectories())
                startDirectorySize += GetDirectorySize(subDirectory, recursive);
        }

        return startDirectorySize; // Return full Size of this Directory.
    }

    public static long GetDirectorySizeParallel(this DirectoryInfo directoryInfo, bool recursive = true)
    {
        var startDirectorySize = default(long);
        if (directoryInfo == null || !directoryInfo.Exists)
            return startDirectorySize; //Return 0 while Directory does not exist.

        //Add size of files in the Current Directory to main size.
        foreach (var fileInfo in directoryInfo.GetFiles())
            Interlocked.Add(ref startDirectorySize, fileInfo.Length);

        if (recursive) //Loop on Sub Direcotries in the Current Directory and Calculate it's files size.
            Parallel.ForEach(directoryInfo.GetDirectories(), subDirectory =>
                Interlocked.Add(ref startDirectorySize, GetDirectorySizeParallel(subDirectory, recursive)));

        return startDirectorySize; //Return full Size of this Directory.
    }
    
    
    public static long DirSize(this DirectoryInfo d) 
    {    
        long size = 0;    
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis) 
        {      
            size += fi.Length;    
        }
        // Add subdirectory sizes.
        DirectoryInfo[] dis = d.GetDirectories();
        foreach (DirectoryInfo di in dis) 
        {
            size += DirSize(di);   
        }
        return size;  
    }
}