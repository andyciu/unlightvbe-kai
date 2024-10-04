namespace unlightvbe_kai_core
{
    /// <summary>
    /// 屬性附加額外紀錄類別
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class PropertyWithRecord<T, U> where T : new() where U : new()
    {
        /// <summary>
        /// 主屬性
        /// </summary>
        public T MainProperty { get; set; }
        /// <summary>
        /// 額外紀錄
        /// </summary>
        public U RecordValue { get; set; }

        public PropertyWithRecord()
        {
            MainProperty = new();
            RecordValue = new();
        }

        public PropertyWithRecord(T mainProperty, U recordValue)
        {
            MainProperty = mainProperty;
            RecordValue = recordValue;
        }
    }
}
