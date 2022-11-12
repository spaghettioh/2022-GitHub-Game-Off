using System.Collections;
using UnityEngine;

// Note that closestPoint is based on the surface of the collider
// and location represents a point in 3d space.
// The gizmos work in the editor.
//
// Create an origin-based cube and give it a scale of (1, 0.5, 3).
// Change the BoxCollider size to (0.8, 1.2, 0.8).  This means that
// collisions will happen when a GameObject gets close to the BoxCollider.
// The ShowClosestPoint.cs script shows spheres that display the location
// and closestPoint.  Try changing the BoxCollider size and the location
// values.

// Attach this to a GameObject that has a Collider component attached
public class ShowClosestPoint : MonoBehaviour
{
    public Transform Clump;
    Collider collider;
    Vector3 closestPoint;
    GameObject AttachPoint;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        collider = Clump.GetComponent<Collider>();
        transform.SetParent(Clump);

        AttachPoint = new GameObject();
        AttachPoint.transform.position = collider.ClosestPoint(transform.position);
        AttachPoint.transform.SetParent(Clump);
        float duration = 0f;

        var startPosition = transform.localPosition;
        while (transform.localPosition != AttachPoint.transform.localPosition)
        {
            //closestPoint = collider.ClosestPoint(transform.position);
            //Vector3 moveTowardVector = closestPoint - transform.position;
            // endpos += (startpos - endpos ).normalized * multiplier 
            transform.localPosition = Vector3.Lerp(
                startPosition, AttachPoint.transform.localPosition, duration / 5f);
            //if (Vector3.Distance(transform.localPosition, AttachPoint.transform.localPosition) <= .01f)
            //{
            //    transform.localPosition = AttachPoint.transform.localPosition;
            //}
            //transform.localPosition +=
            //    (AttachPoint.transform.localPosition - startPosition).normalized * Time.deltaTime;
            duration += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        print($"full attached: {duration}s");
    }

    public void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawWireSphere(AttachPoint.transform.position, 0.1f);
    }
}