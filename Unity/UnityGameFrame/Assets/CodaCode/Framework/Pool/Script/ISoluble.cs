
namespace Coda.Tools
{
    public interface IBaseSoluble
    {
        /// <summary>
        /// Use for data recycle to the pool.
        /// </summary>
        void ClearData();


        /// <summary>
        /// Use for data delete from the pool.
        /// </summary>
        void DeleteData();
    }

    public interface ISoluble : IBaseSoluble
    {
        /// <summary>
        /// Use for data Init.
        /// </summary>
        void ResetData();
    }

    public interface ISoluble<T> : IBaseSoluble
    {
        /// <summary>
        /// Use for data Init.
        /// </summary>
        void ResetData(T t);
    }

    public interface ISoluble<T, V> : IBaseSoluble
    {
        /// <summary>
        /// Use for data Init.
        /// </summary>
        void ResetData(T t, V v);
    }

    public interface ISoluble<T, V, U> : IBaseSoluble
    {
        /// <summary>
        /// Use for data Init.
        /// </summary>
        void ResetData(T t, V v, U u);
    }
}