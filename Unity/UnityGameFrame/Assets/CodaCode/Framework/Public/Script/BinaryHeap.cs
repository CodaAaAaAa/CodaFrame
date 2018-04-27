
using System.Collections.Generic;

namespace Coda.Tools
{
    public class BinaryHeap<T>
    {

        public int length { get { return _infoList.Count; } }


        public void Clear()
        {
            _infoDic.Clear();
            _infoList.Clear();
        }


        public bool IsEmpty()
        {
            return length == 0;
        }
        

        public void Add(T data, int weight)
        {
            _infoList.Add(data);
            _infoDic[data] = new ElementInfo() { weight = weight };

            int theNewIndex = length - 1;
            _infoDic[data].indexInBinaryHeap = theNewIndex;

            while (true)
            {
                int theParentIndex = theNewIndex / 2;
                if (theParentIndex != 0)
                {
                    if (_infoDic[_infoList[theNewIndex - 1]].weight < _infoDic[_infoList[theParentIndex - 1]].weight)
                    {
                        _DataExchange(theNewIndex - 1, theParentIndex - 1);

                        theNewIndex = theParentIndex;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        

        public T Pop()
        {
            T temp = _infoList[0];

            _infoList[0] = _infoList[_infoList.Count - 1];
            _infoList.RemoveAt(_infoList.Count - 1);
            _infoDic.Remove(temp);

            if (IsEmpty())
                return temp;

            int theSelectIndex = 1;
            _infoDic[_infoList[0]].indexInBinaryHeap = 1;

            while (true)
            {
                int theLeftChildIndex = theSelectIndex * 2;
                int theRightChildIndex = theSelectIndex * 2 + 1;

                if (theLeftChildIndex >= length) break;
                else if (theLeftChildIndex == length - 1)
                {
                    if (_infoDic[_infoList[theSelectIndex - 1]].weight > _infoDic[_infoList[theLeftChildIndex - 1]].weight)
                    {
                        _DataExchange(theSelectIndex - 1, theLeftChildIndex - 1);
                        theSelectIndex = theLeftChildIndex;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (theRightChildIndex < length)
                {
                    if (_infoDic[_infoList[theLeftChildIndex - 1]].weight <= _infoDic[_infoList[theRightChildIndex - 1]].weight)
                    {
                        if (_infoDic[_infoList[theSelectIndex - 1]].weight > _infoDic[_infoList[theLeftChildIndex - 1]].weight)
                        {
                            _DataExchange(theLeftChildIndex - 1, theSelectIndex - 1);

                            theSelectIndex = theLeftChildIndex;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (_infoDic[_infoList[theSelectIndex - 1]].weight > _infoDic[_infoList[theRightChildIndex - 1]].weight)
                        {
                            _DataExchange(theRightChildIndex - 1, theSelectIndex - 1);

                            theSelectIndex = theRightChildIndex;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return temp;
        }


        public void Retaxis(T element)
        {
            Retaxis(_infoDic[element].indexInBinaryHeap);
        }
        

        public void Retaxis(int index)
        {
            int theNewIndex = index;
            while (true)
            {
                int theParentIndex = theNewIndex / 2;
                if (theParentIndex != 0)
                {
                    if (_infoDic[_infoList[theNewIndex - 1]].weight < _infoDic[_infoList[theParentIndex - 1]].weight)
                    {
                        _DataExchange(theNewIndex - 1, theParentIndex - 1);

                        theNewIndex = theParentIndex;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }


        #region Private Part

        private class ElementInfo
        {
            public int weight;
            public int indexInBinaryHeap;
        }

        private Dictionary<T, ElementInfo> _infoDic = new Dictionary<T, ElementInfo>();
        private List<T> _infoList = new List<T>();

        private void _DataExchange(int a, int b)
        {
            T temp = _infoList[a];
            _infoList[a] = _infoList[b];
            _infoList[b] = temp;

            _infoDic[_infoList[a]].indexInBinaryHeap = a;
            _infoDic[_infoList[b]].indexInBinaryHeap = b;
        }

        #endregion
    }
}
