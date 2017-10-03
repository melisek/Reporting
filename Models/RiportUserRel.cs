namespace szakdoga.Models
{
    public class RiportUserRel
    {
        public int Id { get; set; }
        public Riport Riport { get; set; }
        public User User { get; set; }

        //TODO: default Value? enum hozzá? Admin (joga van megosztani más felhasználókkal), Írás és Null esetleg null helyett 0?
        public int AuthoryLayer { get; set; }
    }
}