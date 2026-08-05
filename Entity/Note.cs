namespace NoteApi.Entity
{
    public class Note
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public string Icerik { get; set; } = string.Empty;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.UtcNow;
        public string Kategori { get; set; } = string.Empty;
    }
}
