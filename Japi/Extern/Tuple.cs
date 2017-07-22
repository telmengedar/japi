#if FRAMEWORK35
namespace NightlyCode.Japi.Extern {

    /// <summary>
    /// tuple of 2 items
    /// </summary>
    /// <typeparam name="TItem1">type of first item</typeparam>
    /// <typeparam name="TItem2">type of second item</typeparam>
    public class Tuple<TItem1, TItem2> {

        /// <summary>
        /// creates a new <see cref="Tuple{TItem1,TItem2}"/>
        /// </summary>
        /// <param name="item1">first item</param>
        /// <param name="item2">second item</param>
        public Tuple(TItem1 item1, TItem2 item2) {
            Item1 = item1;
            Item2 = item2;
        }

        /// <summary>
        /// first item
        /// </summary>
        public TItem1 Item1 { get; private set; }

        /// <summary>
        /// second item
        /// </summary>
        public TItem2 Item2 { get; private set; }
    }
}
#endif