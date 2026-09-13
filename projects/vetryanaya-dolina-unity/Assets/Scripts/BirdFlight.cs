using UnityEngine;

public class BirdFlight : MonoBehaviour
{
    private Transform[] route;
    private int currentPointIndex = 0;
    private float speed;
    private float rotationSpeed = 5f;

    private Vector3 rotationOffset = new Vector3(0, 90, 0);

    public void InitRoute(Transform[] points, float flightSpeed)
    {
        route = points;
        speed = flightSpeed;
        
        if (route != null && route.Length > 0)
        {
            transform.position = route[0].position;
            currentPointIndex = 1; 
        }
    }

    private void Update()
    {
        if (route == null || route.Length == 0 || currentPointIndex >= route.Length) return;

        Transform targetPoint = route[currentPointIndex];

        Vector3 direction = targetPoint.position - transform.position;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            targetRotation *= Quaternion.Euler(rotationOffset);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            currentPointIndex++;

            if (currentPointIndex >= route.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
