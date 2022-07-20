namespace client.Mvc.Models
{
    public class FileModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }

        public FileModel(string id,
            string path,
            string name,
            string type,
            long size)
        {
            Id = id;
            Path = path;
            Name = name;
            Type = type;
            Size = size;
        }

        public FileModel()
        {
        }
    }
}