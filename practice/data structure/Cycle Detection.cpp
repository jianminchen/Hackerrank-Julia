/*
 code review on June 22, 2018
 https://www.hackerrank.com/challenges/detect-whether-a-linked-list-contains-a-cycle/problem
  Detect loop in a linked list 
  List could be empty also
  Node is defined as 
  struct Node
  {
     int data;
     struct Node *next;
  }
*/
int HasCycle(Node* head)
{
   // Complete this function
   // Do not write the main method
   if(head == NULL || head->next== NULL || head->next->next==NULL)
       return 0; 
   
   Node* slow = head->next; 
   Node* fast = head->next->next; 
    
   while(slow!=NULL && fast !=NULL) 
   {
      if(slow==fast)    
          return 1; 
      slow=slow->next; 
       
      if(fast->next == NULL)
          return 0; 
      fast = fast->next->next; 
   }
    
   return 0; 
}
