using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GymCastillo.Model.Init {
    /// <summary>
    /// Clase que se encarga de manipular el archivo ini.
    /// https://stackoverflow.com/questions/217902/reading-writing-an-ini-file
    /// </summary>
    public class IniFile {

        private readonly string _path;
        private const string Exe = "GymCastillo";

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultS, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Constructor de la clase IniFile.
        /// </summary>
        /// <param name="iniPath">Ruta donde se va a guardar el archivo ini.</param>
        public IniFile(string iniPath = null) {
            _path = new FileInfo(iniPath ?? Exe + ".ini").FullName;
        }

        /// <summary>
        /// Función para leer del archivo ini.
        /// </summary>
        /// <param name="key">Nombre del campo a leer.</param>
        /// <param name="section">Sección del campo a leer (opcional).</param>
        /// <returns></returns>
        public string Read(string key, string section = null) {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section ?? Exe, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        /// <summary>
        /// Función para escribir al archivo ini.
        /// </summary>
        /// <param name="key">Nombre del campo a escribir</param>
        /// <param name="value">Valor a escribir en el campo.</param>
        /// <param name="section">Sección donde se encuentra el campo (opcional).</param>
        public void Write(string key, string value, string section = null) {
            WritePrivateProfileString(section ?? Exe, key, value, _path);
        }

        /// <summary>
        /// Método que borra una key de el ini.
        /// </summary>
        /// <param name="key">La key a eliminar</param>
        /// <param name="section">La sección en la que se encuentra la key.</param>
        public void DeleteKey(string key, string section = null) {
            Write(key, null, section ?? Exe);
        }

        /// <summary>
        /// Método que borra una sección completa del ini.
        /// </summary>
        /// <param name="section"></param>
        public void DeleteSection(string section = null) {
            Write(null, null, section ?? Exe);
        }

        /// <summary>
        /// Función que verifica si una key del archivo ini existe.
        /// </summary>
        /// <param name="key">La key a buscar.</param>
        /// <param name="section">La sección a la que corresponde.</param>
        /// <returns></returns>
        public bool KeyExists(string key, string section = null) {
            return Read(key, section).Length > 0;
        }
    }
}