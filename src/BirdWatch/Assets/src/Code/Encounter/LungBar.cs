using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LungBar : MonoBehaviour
{
    private List<RatingBar> RatingBars = new List<RatingBar>();

    public RatingBar RightmostBar()
    {
        return RatingBars
            .Where(r => r.IsActive())
            .OrderByDescending(r => r.transform.position.x)
            .FirstOrDefault();
    }
    
    public void Clear()
    {
        RatingBars.Clear();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var b = other.GetComponent<RatingBar>();
        if (b != null && !RatingBars.Contains(b))
        {
            RatingBars.Add(b);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        var b = other.GetComponent<RatingBar>();
        if (b != null && RatingBars.Contains(b))
        {
            RatingBars.Remove(b);
        }
    }
}
