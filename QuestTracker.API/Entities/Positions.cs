using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities
{
    //public class ListPosition
    //{

    //}

    //public class TaskPosition
    //{
        
    //}

    /* TODO how to manage List and TaskItem Positions via models & ordered lists of ints
     *  [HttpPost]
public ActionResult UpdateSort(int[] ids)
{
    if (ids != null && ids.Length > 0)
    {
        using (var con = Connection.OpenSql())
        {
            for (var i = 0; i < ids.Length; ++i)
                con.Execute("UPDATE Table SET SortOrder = @sort WHERE Id = @id", new { sort = i, id = ids[i] });
        }
    }
    return new HttpStatusCodeResult(200);
} 

    Ideal results of GET
    All Lists for User: Array of List objects, in order:    
    json (List Object)
{
  "id": 131232, -> Id of Containing List
  "values": [123, 234, 345, 456], Id of Contained Tasks
  "revision": 1,
  "type": "list_position"
}
    

 */
}