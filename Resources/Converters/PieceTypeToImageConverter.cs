using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartChess.Resources.Converters
{
    public class PieceTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ChessPiece piece)
            {
                // Возвращает путь к изображению из свойства ImagePath фигуры
                return piece.ImagePath;
            }
            // Возвращаем путь к пустой клетке или null
            return null; // Или путь к изображению "пустой" клетки, если такое есть
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}