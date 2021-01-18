using System;

namespace _001_Enumerable
{
    class Program
    {
        static void Main(string[] args)
        {
            ListNode node1 = new ListNode { value = 10 };
            ListNode node2 = new ListNode { value = 20 };
            node1.next = node2;
            ListNode node3 = new ListNode { value = 30 };
            node2.next = node3;
            ListNode node4 = new ListNode { value = 40 };
            node3.next = node4;
        }
    }

    class ListNode
    {
        public int value;
        public ListNode next;

        public override string ToString() => value.ToString();
    }
}
