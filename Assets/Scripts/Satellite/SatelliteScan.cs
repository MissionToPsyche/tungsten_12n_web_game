// using UnityEngine;

// public class SatelliteScan : MonoBehaviour
// {
//     private RaycastHit2D hit;
//     private Vector2 rayDirection;
//     private bool isScanning = false;
//     private GameObject parentAsteroid;


//     // -------------------------------------------------------------------
//     // Handle events

//     public void OnSatelliteScan(bool scanning)
//     {
//         shouldDrawBlueRay = true;

//         if (currentState == State.Manual && Mathf.Approximately(transform.velocity.magnitude, 0))
//         {
//             if (scanning)
//             {
//                 StartScanning();
//             }
//             else
//             {
//                 StopScanning();
//             }
//         }
//     }

//     // -------------------------------------------------------------------
//     // API


//     // -------------------------------------------------------------------
//     // Class

//     public void StartScanning(Vector3 satellitePosition)
//     {
//         isScanning = true;
//         Debug.Log("Scan started");
//         UpdateRayDirection(satellitePosition); // Update direction each time scan starts
//         hit = Physics2D.Raycast(satellitePosition, rayDirection, Mathf.Infinity, LayerMask.GetMask("DiscoveredResource"));
//     }

//     public void StopScanning()
//     {
//         isScanning = false;
//         Debug.Log("Scan stopped");
//     }

//     public void UpdateScanningRay(Vector3 satellitePosition)
//     {
//         if (!isScanning) return;

//         UpdateRayDirection(satellitePosition); // Continuously update direction

//         if (hit.collider != null)
//         {
//             Debug.DrawRay(satellitePosition, rayDirection * hit.distance, Color.green);
//         }
//         else
//         {
//             Debug.DrawRay(satellitePosition, rayDirection * 100, Color.red); // Draw far enough for visibility
//         }
//     }

//     private void UpdateRayDirection(Vector3 satellitePosition)
//     {
//         if (parentAsteroid != null)
//         {
//             rayDirection = (parentAsteroid.transform.position - satellitePosition).normalized;
//         }
//     }

//     public void SetParentAsteroid(GameObject asteroid)
//     {
//         parentAsteroid = asteroid;
//     }
// }
