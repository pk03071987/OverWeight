using UnityEngine;
using System.Collections;

public class VectorOperator
{
    public static Vector3 getCameraCentrPoint (Camera cam)
    {
        return new Vector3( (cam.rect.x + 0.5f*cam.rect.width)*Screen.width, (cam.rect.y + 0.5f*cam.rect.height)*Screen.height, 0.0f );
    }
    public static bool checkCameraSreen (Camera cam)
    {
        Vector3 centrePoint = getCameraCentrPoint(cam);
        Vector3 screenPoint = Input.mousePosition;
        
        return (Mathf.Abs(centrePoint.x - screenPoint.x) <= 0.5f * cam.rect.width * Screen.width) && (Mathf.Abs(centrePoint.y - screenPoint.y) <= 0.5f * cam.rect.height * Screen.height);
    }
	public static float getRayDistance (Ray ray, Vector3 point)
	{
		return getRayNormal(ray, point).magnitude;
	}
	public static Vector3 getRayPoint (Ray ray, Vector3 point)
	{
		return ray.origin + Vector3.Project(point - ray.origin, ray.direction);
	}
	public static Vector3 getRayNormal (Ray ray, Vector3 point)
	{
		Vector3 origin = ray.origin;
		Vector3 direction = ray.direction;
		Vector3 delta = point - origin;
		Vector3 project = Vector3.Project(delta, direction);
		Vector3 normal = delta- project;
		return normal;
	}
	public static Vector3 getPerpendicularXZ(Vector3 normal)
	{
		return new Vector3(-normal.z, 0.0f, normal.x);
	}
    public static Vector3 getProjectPointOnAxis(Vector3 point, Vector3 pointOnAxis, Vector3 axis)
    {
        return pointOnAxis + Vector3.Project(point - pointOnAxis, axis);
    }
    public static Vector3 getProjectPointOnPlane(Vector3 point, Vector3 pointOnPlane, Vector3 planeNormal)
    {
        return point - Vector3.Project(point - pointOnPlane, planeNormal);
    }
	public static Vector3 getProjectXZ (Vector3 currentVector, bool saveLength)
	{
		Vector3 projectXZ = new Vector3(currentVector.x, 0.0f, currentVector.z);
		return saveLength? currentVector.magnitude*projectXZ.normalized: projectXZ;
	}
	public static Vector3 getPerpendicularVectorVector (Vector3 current, Vector3 normal)
	{
		return current - Vector3.Project(current, normal);
	}
	public static Vector3 getBallVelocity (float radius, Vector3 currentVelocity, Vector3 currentPosition, LayerMask hitCheckMask, float maxDistance, 
                                           ref Vector3 hitBallVelocity, ref Collider hitCollider, float rotationDisplacementX, float rotationDisplacementY)
	{
		Vector3 newVelocity = Vector3.zero;

		Ray ray = new Ray(currentPosition - radius*currentVelocity.normalized, currentVelocity.normalized);
		RaycastHit hit;
		hitCollider = null;
		if(Physics.SphereCast(ray, radius, out hit, maxDistance, hitCheckMask))
		{
		    hitCollider = hit.collider;
            Vector3 perependicular = getPerpendicularXZ(hit.normal);// getPerpendicularXZ(hit.normal);

            float tangets = Vector3.Dot(perependicular, currentVelocity.normalized);
            //Debug.Log("tangets " + tangets);
            if(tangets < 0.0f)
            {
                perependicular = -perependicular;
                //tangets = -tangets;
            }

			if(hitCollider.GetComponent<Rigidbody>())
			{
                newVelocity = Vector3.Project(currentVelocity, perependicular);

                float yFactor = (1.0f - Mathf.Abs( rotationDisplacementY ));
                float orient = tangets >= 0.0f? -0.15f: 0.15f;
               
                newVelocity = newVelocity.magnitude * (orient * (yFactor* rotationDisplacementX - 0.0f*tangets * 2.9f * rotationDisplacementY)*(currentVelocity.normalized - newVelocity.normalized ) + newVelocity.normalized).normalized;
               
				hitBallVelocity = -( newVelocity - currentVelocity ).magnitude*hit.normal;
				hitBallVelocity = getProjectXZ( hitBallVelocity , true);

                hitBallVelocity = hitBallVelocity.magnitude * (orient * (0.9f*yFactor* rotationDisplacementX - 0.0f*tangets * 0.5f * rotationDisplacementY)*(currentVelocity.normalized - hitBallVelocity.normalized ) + hitBallVelocity.normalized).normalized;
			}
			else
			{
				newVelocity = currentVelocity - 2.0f*Vector3.Project(currentVelocity, -hit.normal);
                float yFactor = (1.0f - Mathf.Abs( rotationDisplacementY ));
                float orient = rotationDisplacementX >= 0.0f? (tangets >= 0.0? 0.95f: -2.0f):(tangets >= 0.0? 2.0f: -0.95f);
                newVelocity = newVelocity.magnitude * (orient * (yFactor* rotationDisplacementX + tangets * 0.95f * rotationDisplacementY)*(perependicular.normalized - newVelocity.normalized ) + newVelocity.normalized).normalized;
				hitBallVelocity = Vector3.zero;	
			}

			string collisionName = LayerMask.LayerToName(hitCollider.gameObject.layer);
			if(collisionName == "Wall")
			newVelocity = getProjectXZ( newVelocity , true );
		}

		return newVelocity;
	}
    /*public static Vector3 getRotationXZ (Vector3 currentPoint,  Vector3 currentVector, float angle )
    {
        return Vector3.one;
    }*/
    public static Vector3 getBallWallVelocity (float radius, Vector3 currentVelocity, Vector3 currentPosition, LayerMask hitCheckMask, float maxDistance, Transform arrow, Vector3 angularVelocityY)
	{
		Vector3 newVelocity = Vector3.zero;
		
		Ray ray = new Ray(currentPosition, currentVelocity.normalized);
		RaycastHit hit;
		if(Physics.SphereCast(ray, radius, out hit, maxDistance, hitCheckMask))
		{
            Vector3 forceDirection = Vector3.zero;

            if(angularVelocityY.magnitude != 0.0f)
            {
                Transform arrowInst = Transform.Instantiate(arrow) as Transform;
                arrowInst.position = hit.point + 5.0f*radius*Vector3.up;
                forceDirection = -Vector3.Cross(angularVelocityY.normalized , getProjectXZ( -hit.normal, false));
                arrowInst.LookAt(arrowInst.position + forceDirection);
            }
            Vector3 project = Vector3.Project(currentVelocity, -hit.normal);
            newVelocity = currentVelocity - 2.0f*project;

            newVelocity = getProjectXZ( newVelocity , true );
            if(angularVelocityY.magnitude != 0.0f)
            {
                newVelocity = newVelocity.magnitude*(newVelocity + project.magnitude*angularVelocityY.magnitude*forceDirection.normalized).normalized;
            }
		}
		
		return newVelocity;
	}
    public static Vector3 getVectorSqale (Vector3 a, Vector3 b)
    {
        return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
    }
	public static void KeepInCube (Transform cube, float radius, Transform sphere)
	{
		if(!sphereInCube(sphere.position, radius, cube))
		{
			Vector3 _ballLocalPosition = getLocalPosition(cube, sphere.position);
			Debug.LogWarning("_ballLocalPosition " + _ballLocalPosition);
			float displacementX1 = _ballLocalPosition.x + radius - 0.5f*cube.localScale.x;
			float displacementX2 = _ballLocalPosition.x - radius + 0.5f*cube.localScale.x;

			if( displacementX1 > 0.0f )
			{
				sphere.position -= displacementX1*cube.right;
			}
			else
				if( displacementX2 < 0.0f )
			{
				sphere.position -= displacementX2*cube.right;
			}

			float displacementZ1 = _ballLocalPosition.z + radius - 0.5f*cube.localScale.z;
			float displacementZ2 = _ballLocalPosition.z - radius + 0.5f*cube.localScale.z;

			if( displacementZ1 > 0.0f )
			{
				sphere.position -= displacementZ1*cube.forward;
			}
			else
				if( displacementZ2 < 0.0f )
			{
				sphere.position -= displacementZ2*cube.forward;
			}
		}
	}
	public static void MoveBallInQuad (Transform cube, float radius, Vector3 hitPoint, ref Vector3 position)
	{
		if(sphereInCube(hitPoint, radius, cube))
		{
			position = hitPoint;
		}
		else
		{
			Vector3 _ballMoveLocalPosition = getLocalPosition(cube, hitPoint);
			
			if( !(Mathf.Abs( _ballMoveLocalPosition.x ) <= -radius + 0.5f*cube.localScale.x) )
			{
				if(Mathf.Abs( _ballMoveLocalPosition.z ) <= -radius + 0.5f*cube.localScale.z)
				{
					Vector3 ballMovePositionX = _ballMoveLocalPosition.x > 0.0f? getWordPosition(cube, (-radius + 0.5f*cube.localScale.x)*Vector3.right ):
						getWordPosition(cube, (radius - 0.5f*cube.localScale.x)*Vector3.right);
					position = new Vector3(ballMovePositionX.x, position.y, hitPoint.z);
				}
			}
			else
			{
				if(Mathf.Abs( _ballMoveLocalPosition.x ) <= -radius + 0.5f*cube.localScale.x)
				{
					Vector3 ballMovePositionZ = _ballMoveLocalPosition.z > 0.0f? getWordPosition(cube, (-radius + 0.5f*cube.localScale.z)*Vector3.forward ):
						getWordPosition(cube, (radius - 0.5f*cube.localScale.z)*Vector3.forward);
					position = new Vector3(hitPoint.x, position.y, ballMovePositionZ.z);
				}
			}
		}
		
		
	}
	public static Vector3 getLocalPosition(Transform parent, Vector3 wordPosition)
	{
		Vector3 localPosition = wordPosition - parent.position;
		return  new Vector3( Vector3.Dot(localPosition, parent.right),
		                    Vector3.Dot(localPosition, parent.up),
		                    Vector3.Dot(localPosition, parent.forward)
		                   );
	}
	public static Vector3 getWordPosition(Transform parent, Vector3 localPosition)
	{
		return parent.position + localPosition.x*parent.right + localPosition.y*parent.up + localPosition.z*parent.forward;
	}
	public static void keepToFace(Transform objectTokeep, Transform keeper, ref Vector3 normal, ref Vector3 point, Vector3 wallNormal, Vector3 wallPoint, Vector3 wordUp)
	{
		keeper.position = point;
		keeper.LookAt(point + normal, wordUp);
		Transform parent = objectTokeep.parent;
		objectTokeep.parent = keeper;
		keeper.position = wallPoint;
		keeper.LookAt(wallPoint - wallNormal, wordUp);
		objectTokeep.parent = parent;
		point = wallPoint;
		normal = -wallNormal;
	}
	public static float localCylindricalAngle(Transform Parent, Vector3 Child)
	{
		Vector3 localPosition = Child - Parent.position;
		float dot = Vector3.Dot(localPosition.normalized, Parent.forward);
		float angle;
		if(Vector3.Dot(localPosition.normalized, Parent.up) >= 0)
		angle = (180/Mathf.PI)* Mathf.Acos( dot );
		else
		angle = -(180/Mathf.PI)* Mathf.Acos( dot );
		return angle;
	}
	public static Vector3 localCylindricalPosition(Transform Parent, Vector3 Child)
	{
		Vector3 localPosition = Child - Parent.position;
		float radius = Mathf.Sqrt( Mathf.Pow( Vector3.Dot(localPosition, Parent.right) , 2) + Mathf.Pow( Vector3.Dot(localPosition, Parent.forward) , 2));
		float angle = localCylindricalAngle(Parent, Child);
		float height = Vector3.Dot(localPosition, Parent.up);
		return new Vector3( radius, angle, height );
	}
	public static bool pointInAngle(Vector3 point, Transform cylindr, float min_angle, float max_angle)
	{
		float localCylindricalAng = localCylindricalAngle(cylindr, point);
		bool in_angle = localCylindricalAng >= min_angle && localCylindricalAng <= max_angle;
		return in_angle;
	}
	
	public static bool sphereInCube (Vector3 point, float radius,  Transform cube)
	{
		Vector3 localPos = getLocalPosition(cube, point);
		Vector3 cubeSqale = cube.lossyScale;
		return Mathf.Abs(localPos.x) + radius <= 0.5f*cubeSqale.x && Mathf.Abs(localPos.y) + radius <= 0.5f*cubeSqale.y && Mathf.Abs(localPos.z) + radius <= 0.5f*cubeSqale.z;
	}
	public static bool sphereCheckCube (Vector3 point, float radius,  Transform cube)
	{
		return sphereInCube ( point, -radius,  cube);
	}
	/*public static bool CheckSphereCast (Vector3 point, float radius, MechAILAb.PDBody cube)
	{
		Vector3 localPos = MechAILAb.PdVector.GetPdVector3( getLocalPosition(cube.transform, point));
		Vector3 boxeScale = MechAILAb.PdVector.GetPdVector3( cube.size);
		return MechAILAb.PdFloat.CompareTo( Mathf.Abs(localPos.x) - radius , 0.5f*boxeScale.x) <= 0 
			&& MechAILAb.PdFloat.CompareTo( Mathf.Abs(localPos.y) - radius , 0.5f*boxeScale.y) <= 0
		    && MechAILAb.PdFloat.CompareTo( Mathf.Abs(localPos.z) - radius , 0.5f*boxeScale.z) <= 0;
	}*/
	public static bool sphereInCylindr (Vector3 point, float radius,  Transform cylindr, float min_angle, float max_angle)
	{
		Vector3 localCylindricalPos = localCylindricalPosition(cylindr, point);
		bool in_radius = Mathf.Abs(localCylindricalPos.x) - radius <= 0.5f*cylindr.lossyScale.x;
		bool in_angle = localCylindricalPos.y >= min_angle && localCylindricalPos.y <= max_angle;
		bool in_height = Mathf.Abs(localCylindricalPos.z) - radius <= cylindr.lossyScale.y;
		return in_radius && in_angle && in_height;
	}
	public static bool sphereInCylindr (Vector3 point, float radius,  Transform cylindr)
	{
		Vector3 localCylindricalPos = localCylindricalPosition(cylindr, point);
		bool in_radius = Mathf.Abs(localCylindricalPos.x) - radius <= 0.5f*cylindr.lossyScale.x;
		bool in_height = Mathf.Abs(localCylindricalPos.z) - radius <= cylindr.lossyScale.y;
		return in_radius && in_height;
	}
	public static bool cubeCheckCube (Transform cube1, Transform cube2)
	{
		Vector3 pos1 = cube1.position;
		Vector3 pos2 = cube2.position;
				
		Vector3 localCube1Pos1 = 0.5f*cube1.lossyScale.x * cube1.right + 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube2Pos1 = 0.5f*cube2.lossyScale.x * cube2.right + 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		
		float localCube1Scale = localCube1Pos1.magnitude;
		float localCube2Scale = localCube2Pos1.magnitude;
		
		if(Vector3.Distance(pos1, pos2) > localCube1Scale + localCube2Scale)
		return false;
		Vector3 localCube1Pos2 = -0.5f*cube1.lossyScale.x * cube1.right + 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos3 = 0.5f*cube1.lossyScale.x * cube1.right - 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos4 = -0.5f*cube1.lossyScale.x * cube1.right - 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		
		Vector3 localCube2Pos2 = -0.5f*cube2.lossyScale.x * cube2.right + 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos3 = 0.5f*cube2.lossyScale.x * cube2.right - 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos4 = -0.5f*cube2.lossyScale.x * cube2.right - 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		
		
		Vector3 cube1Pos1 = pos1 + localCube1Pos1;
		Vector3 cube1Pos2 = pos1 - localCube1Pos1;
		Vector3 cube1Pos3 = pos1 + localCube1Pos2;
		Vector3 cube1Pos4 = pos1 - localCube1Pos2;
		Vector3 cube1Pos5 = pos1 + localCube1Pos3;
		Vector3 cube1Pos6 = pos1 - localCube1Pos3;
		Vector3 cube1Pos7 = pos1 + localCube1Pos4;
		Vector3 cube1Pos8 = pos1 - localCube1Pos4;
		
		Vector3 cube2Pos1 = pos2 + localCube2Pos1;
		Vector3 cube2Pos2 = pos2 - localCube2Pos1;
		Vector3 cube2Pos3 = pos2 + localCube2Pos2;
		Vector3 cube2Pos4 = pos2 - localCube2Pos2;
		Vector3 cube2Pos5 = pos2 + localCube2Pos3;
		Vector3 cube2Pos6 = pos2 - localCube2Pos3;
		Vector3 cube2Pos7 = pos2 + localCube2Pos4;
		Vector3 cube2Pos8 = pos2 - localCube2Pos4;

		return  sphereInCube(cube1Pos1, 0 , cube2) ||
			    sphereInCube(cube1Pos2, 0 , cube2) ||
				sphereInCube(cube1Pos3, 0 , cube2) ||
				sphereInCube(cube1Pos4, 0 , cube2) ||
				sphereInCube(cube1Pos5, 0 , cube2) ||
				sphereInCube(cube1Pos6, 0 , cube2) ||
				sphereInCube(cube1Pos7, 0 , cube2) ||
				sphereInCube(cube1Pos8, 0 , cube2) ||
				
				sphereInCube(cube2Pos1, 0 , cube1) ||
				sphereInCube(cube2Pos2, 0 , cube1) ||
				sphereInCube(cube2Pos3, 0 , cube1) ||
				sphereInCube(cube2Pos4, 0 , cube1) ||
				sphereInCube(cube2Pos5, 0 , cube1) ||
				sphereInCube(cube2Pos6, 0 , cube1) ||
				sphereInCube(cube2Pos7, 0 , cube1) ||
				sphereInCube(cube2Pos8, 0 , cube1) ||
				sphereInCube(cube1.position, 0 , cube2) || 
				sphereInCube(cube2.position, 0 , cube1);
	}
	public static bool cubeInCube (Transform cube1, Transform cube2)
	{
		Vector3 pos1 = cube1.position;
		Vector3 pos2 = cube2.position;
		
		Vector3 localCube1Pos1 = 0.5f*cube1.lossyScale.x * cube1.right + 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube2Pos1 = 0.5f*cube2.lossyScale.x * cube2.right + 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		
		float localCube1Scale = localCube1Pos1.magnitude;
		float localCube2Scale = localCube2Pos1.magnitude;
		
		if(Vector3.Distance(pos1, pos2) > localCube1Scale + localCube2Scale)
			return false;

		Vector3 localCube1Pos2 = -0.5f*cube1.lossyScale.x * cube1.right + 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos3 = -0.5f*cube1.lossyScale.x * cube1.right - 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos4 = 0.5f*cube1.lossyScale.x * cube1.right - 0.5f*cube1.lossyScale.y*cube1.up + 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos5 = 0.5f*cube1.lossyScale.x * cube1.right + 0.5f*cube1.lossyScale.y*cube1.up - 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos6 = -0.5f*cube1.lossyScale.x * cube1.right + 0.5f*cube1.lossyScale.y*cube1.up - 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos7 = -0.5f*cube1.lossyScale.x * cube1.right - 0.5f*cube1.lossyScale.y*cube1.up - 0.5f*cube1.lossyScale.z*cube1.forward;
		Vector3 localCube1Pos8 = 0.5f*cube1.lossyScale.x * cube1.right - 0.5f*cube1.lossyScale.y*cube1.up - 0.5f*cube1.lossyScale.z*cube1.forward;
	
		Vector3 localCube2Pos2 = -0.5f*cube2.lossyScale.x * cube2.right + 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos3 = -0.5f*cube2.lossyScale.x * cube2.right - 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos4 = 0.5f*cube2.lossyScale.x * cube2.right - 0.5f*cube2.lossyScale.y*cube2.up + 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos5 = 0.5f*cube2.lossyScale.x * cube2.right + 0.5f*cube2.lossyScale.y*cube2.up - 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos6 = -0.5f*cube2.lossyScale.x * cube2.right + 0.5f*cube2.lossyScale.y*cube2.up - 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos7 = -0.5f*cube2.lossyScale.x * cube2.right - 0.5f*cube2.lossyScale.y*cube2.up - 0.5f*cube2.lossyScale.z*cube2.forward;
		Vector3 localCube2Pos8 = 0.5f*cube2.lossyScale.x * cube2.right - 0.5f*cube2.lossyScale.y*cube2.up - 0.5f*cube2.lossyScale.z*cube2.forward;

		Vector3 cube1Pos1 = pos1 + localCube1Pos1;
		Vector3 cube1Pos2 = pos1 + localCube1Pos2;
		Vector3 cube1Pos3 = pos1 + localCube1Pos3;
		Vector3 cube1Pos4 = pos1 + localCube1Pos4;
		Vector3 cube1Pos5 = pos1 + localCube1Pos5;
		Vector3 cube1Pos6 = pos1 + localCube1Pos6;
		Vector3 cube1Pos7 = pos1 + localCube1Pos7;
		Vector3 cube1Pos8 = pos1 + localCube1Pos8;
		
		Vector3 cube2Pos1 = pos2 + localCube2Pos1;
		Vector3 cube2Pos2 = pos2 + localCube2Pos2;
		Vector3 cube2Pos3 = pos2 + localCube2Pos3;
		Vector3 cube2Pos4 = pos2 + localCube2Pos4;
		Vector3 cube2Pos5 = pos2 + localCube2Pos5;
		Vector3 cube2Pos6 = pos2 + localCube2Pos6;
		Vector3 cube2Pos7 = pos2 + localCube2Pos7;
		Vector3 cube2Pos8 = pos2 + localCube2Pos8;
		
		return (sphereInCube(cube1Pos1, 0 , cube2) &&
			    sphereInCube(cube1Pos2, 0 , cube2) &&
				sphereInCube(cube1Pos3, 0 , cube2) &&
				sphereInCube(cube1Pos4, 0 , cube2) &&
				sphereInCube(cube1Pos5, 0 , cube2) &&
				sphereInCube(cube1Pos6, 0 , cube2) &&
				sphereInCube(cube1Pos7, 0 , cube2) &&
				sphereInCube(cube1Pos8, 0 , cube2)) ||
				
			   (sphereInCube(cube2Pos1, 0 , cube1) &&
				sphereInCube(cube2Pos2, 0 , cube1) &&
				sphereInCube(cube2Pos3, 0 , cube1) &&
				sphereInCube(cube2Pos4, 0 , cube1) &&
				sphereInCube(cube2Pos5, 0 , cube1) &&
				sphereInCube(cube2Pos6, 0 , cube1) &&
				sphereInCube(cube2Pos7, 0 , cube1) &&
				sphereInCube(cube2Pos8, 0 , cube1));
	}
	public static void DrawGizmosCube (Transform cube)
	{
		Vector3 pos = cube.position;
		
		Vector3 localPos1 = 0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localPos2 = -0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localPos3 = -0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localPos4 = 0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;

		Vector3 localPos5 = 0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localPos6 = -0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localPos7 = -0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localPos8 = 0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;


		Gizmos.DrawLine (pos + localPos1, pos + localPos2);
		Gizmos.DrawLine (pos + localPos2, pos + localPos3);
		Gizmos.DrawLine (pos + localPos3, pos + localPos4);
		Gizmos.DrawLine (pos + localPos4, pos + localPos1);

		Gizmos.DrawLine (pos + localPos5, pos + localPos6);
		Gizmos.DrawLine (pos + localPos6, pos + localPos7);
		Gizmos.DrawLine (pos + localPos7, pos + localPos8);
		Gizmos.DrawLine (pos + localPos8, pos + localPos5);

		Gizmos.DrawLine (pos + localPos1, pos + localPos5);
		Gizmos.DrawLine (pos + localPos2, pos + localPos6);
		Gizmos.DrawLine (pos + localPos3, pos + localPos7);
		Gizmos.DrawLine (pos + localPos4, pos + localPos8);
	
	}
	public static Vector3[] getCubePoints (Transform cube)
	{
		Vector3 pos = cube.position;

		Vector3[] points = new Vector3[8];

		Vector3 localCube1Pos1 = 0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localCube1Pos2 = -0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localCube1Pos3 = -0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localCube1Pos4 = 0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up + 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localCube1Pos5 = 0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localCube1Pos6 = -0.5f*cube.lossyScale.x * cube.right + 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localCube1Pos7 = -0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;
		Vector3 localCube1Pos8 = 0.5f*cube.lossyScale.x * cube.right - 0.5f*cube.lossyScale.y*cube.up - 0.5f*cube.lossyScale.z*cube.forward;

		points[0] = pos + localCube1Pos1;
		points[1] = pos + localCube1Pos2;
		points[2] = pos + localCube1Pos3;
		points[3] = pos + localCube1Pos4;

		points[4] = pos + localCube1Pos5;
		points[5] = pos + localCube1Pos6;
		points[6] = pos + localCube1Pos7;
		points[7] = pos + localCube1Pos8;

		return points;
	}
}
