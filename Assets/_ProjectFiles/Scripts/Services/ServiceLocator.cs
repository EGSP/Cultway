using System;
using System.Collections.Generic;
using Egsp.Extensions.Linq;

namespace Game.Services
{
    public class ServiceLocator
    {
        
        /// <summary>
        /// Список с инструментами
        /// </summary>
        private readonly List<object> _services;

        public ServiceLocator()
        {
            _services = new List<object>();
        }
        
        /// <summary>
        /// Добавление инструмента.
        /// </summary>
        public bool Add(object tool)
        {
            // Если инструмент не был добавлен
            if (!_services.Contains(tool))
            {
                _services.Add(tool);
                return true;
            }
        
            return false;
        }
        
        /// <summary>
        /// Удаление инструмента по ссылке
        /// </summary>
        public void Remove(object tool)
        {
            _services.Remove(tool);
        }
        
        public  void Remove<T>() where T: class
        {
            var coincidence = GetTool<T>();
        
            if (coincidence != null)
                _services.Remove(coincidence);
        }
        
        /// <summary>
        /// Удаление инструмента по условию
        /// </summary>
        public void Remove<T>(Predicate<T> predicate) where T: class
        {
            var predicated = GetTool<T>();
        
            if (predicated != null)
                _services.Remove(predicated);
        }
        
        /// <summary>
        /// Получение инструмента по типу. Возвращает первое совпадение.
        /// Может вернуть null.
        /// </summary>
        public T GetTool<T>() where T : class
        {
            return _services.FindType<T>();
        }
        
        /// <summary>
        /// Получение инструмента по типу и условию. Возвращает первое совпадение.
        /// Может вернуть null.
        /// </summary>
        public T GetTool<T>(Predicate<T> predicate) where T : class
        {
            var toolsT = _services.FindTypes<T>();
        
            for (var i = 0; i < toolsT.Count; i++)
            {
                var toolT = toolsT[i];
                var predicated = predicate(toolsT[i]);
        
                // Если инструмент удовлетворяет условиям
                if (predicated == true)
                    return toolT;
            }
        
            return default(T);
        }        
    }
}