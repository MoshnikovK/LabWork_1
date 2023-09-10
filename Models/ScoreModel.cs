namespace Game2048.Models
{
    public class ScoreModel : BaseModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public virtual UserModel User { get; set; }
        public int Score { get; set; }
    }
}