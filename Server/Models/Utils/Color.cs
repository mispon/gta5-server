namespace gta_mp_server.Models.Utils {
    /// <summary>
    /// Модель цвета
    /// </summary>
    public class Color {
        public Color(int red, int green, int blue, int bright = 120) {
            Red = red;
            Green = green;
            Blue = blue;
            Bright = bright;
        }

        public int Bright { get; private set; }
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }
    }
}