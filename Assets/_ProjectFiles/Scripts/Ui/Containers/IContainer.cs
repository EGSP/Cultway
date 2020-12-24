using System;
using UnityEngine;

namespace Game.Ui
{
    
    public interface IContainer
    {
        /// <summary>
        /// Создает экземпляр объекта, помещает его в контейнер и возвращает на него ссылку.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        TObject Put<TObject>(TObject prefab) where TObject : class;
    }
}