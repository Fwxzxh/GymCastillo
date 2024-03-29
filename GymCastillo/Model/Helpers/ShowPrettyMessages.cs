﻿using System.Windows;

namespace GymCastillo.Model.Helpers {
    /// <summary>
    /// Clase que se encarga de Mostrar mensajes de error bonitos.
    /// </summary>
    public static class ShowPrettyMessages {

        /// <summary>
        /// Método que muestra un mensaje de error con un botón de OK
        /// </summary>
        /// <param name="message">El mensaje a mostrar en el cuadro de texto.</param>
        /// <param name="title">El título de la ventana.</param>
        public static void ErrorOk(string message, string title) {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Método que muestra un mensaje de warning con un botón de OK.
        /// </summary>
        /// <param name="message">El mensaje a mostrar en el cuadro de texto.</param>
        /// <param name="title">El título de la ventana.</param>
        public static void WarningOk(string message, string title) {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Método que muestra un mensaje de información con un botón de Ok.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public static void InfoOk(string message, string title) {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Método que muestra un mensaje de pregunta y botones de Si y No.
        /// </summary>
        /// <param name="message">El mensaje a mostrar en el cuadro de texto.</param>
        /// <param name="title">El título de la ventana.</param>
        /// <returns><c>true</c> si el usuario le da que si, <c>false</c> en caso contrario.</returns>
        public static bool QuestionYesNo(string message, string title) {
            var res = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return res == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Método que muestra un mensaje de excito y un botón de ok.
        /// </summary>
        /// <param name="message">El mensaje a mostrar en el cuadro de texto.</param>
        /// <param name="title">El titulo de la ventana.</param>
        /// <returns></returns>
        public static void NiceMessageOk(string message, string title) {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.None);
        }
    }
}