namespace Coypu
{
    /// <summary>
    /// The scope of an element already found in the document, therefore not deferred. 
    /// 
    /// If this element becomes stale then using this scope will not try to refind the element but 
    /// will raise a MissingHtmlException immediately.
    /// </summary>
    public class SnapshotElementScope : DeferredElementScope
    {
        private readonly ElementFound _elementFound;

        internal SnapshotElementScope(ElementFound elementFound, DriverScope scope) : base(null, scope)
        {
            _elementFound = elementFound;
        }

        public override ElementFound Now()
        {
            if (_elementFound.Stale)
                throw new MissingHtmlException(string.Format("Snapshot element scope has become stale. {0}",_elementFound));
            
            return _elementFound;
        }
    }
}