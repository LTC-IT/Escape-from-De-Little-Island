//////////////////////////////////////////////////////////////
// Joystick.cs
// Penelope iPhone Tutorial
//
// Joystick creates a movable joystick (via GUITexture) that 
// handles touch input, taps, and phases. Dead zones can control
// where the joystick input gets picked up and can be normalized.
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;


[RequireComponent (typeof (GUITexture))]
public class Joystick : MonoBehaviour {
	
	// A simple class for bounding how far the GUITexture will move
	private class Boundary 
	{
		public Vector2 min = Vector2.zero;
		public Vector2 max = Vector2.zero;
	}
	
	static private Joystick[] joysticks; 					// A static collection of all joysticks
	static private float tapTimeDelta = 0.3f;				// Time allowed between taps
	static private bool enumeratedJoysticks = false;
	
	public bool normalize;									// Normalize output after the dead-zone?
	public Vector2 position;								// [-1, 1] in x,y
	public Vector2 deadZone = Vector2.zero;					// Control when position is output
	public int tapCount;									// Current tap count
	
	
	private float tapTimeWindow;							// How much time there is left for a tap to occur
	private int lastFingerId = -1;							// Finger last used for this joystick
	private GUITexture gui;									// Joystick graphic
	private Rect defaultRect;								// Default position / extents of the joystick graphic
	private Vector2 guiTouchOffset; 						// Offset to apply to touch input
	private Vector2 guiCenter;								// Offset to apply to touch input
	private Boundary guiBoundary = new Boundary();			// Boundary for joystick graphic
	

	private void LatchedFinger(int fingerId)
	{
		// If another joystick has latched this finger, then we must release it
		if ( lastFingerId == fingerId )
			Reset();
	}
	
	public void Disable()
	{
		gameObject.active = false;
		enumeratedJoysticks = false;
	}
	
	private void Reset() 
	{
		// Release the finger control and set the joystick back to the default position
		gui.pixelInset = defaultRect;
		lastFingerId = -1;
	}
	
	// Use this for initialization
	void Start () 
	{
		// Cache this component at startup instead of looking up every frame
		gui = GetComponent<GUITexture>();
		
		// Store the default rect for the gui, so we can snap back to it
		defaultRect = gui.pixelInset;
		
		// This is an offset for touch input to match with the top left
		// corner of the GUI
		guiTouchOffset.x = defaultRect.width * 0.5f;
		guiTouchOffset.y = defaultRect.width * 0.5f;
		
		// Cache the center of the GUI, since it doesn't change
		guiCenter.x = defaultRect.x + guiTouchOffset.x;
		guiCenter.y = defaultRect.y + guiTouchOffset.y;
		
		// Let's build the GUI boundary, so we can clamp joystick movement
		guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
		guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
		guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
		guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if ( !enumeratedJoysticks ) 
		{
			// Collect all joysticks in the game, so we can relay finger latching messages
			joysticks = (Joystick[]) FindObjectsOfType(typeof(Joystick));
			enumeratedJoysticks = true;
			
		}
		
		int count = Input.touchCount;
		
		// Adjust the tap time window while it still available
		if ( tapTimeWindow > 0 ) 
			tapTimeWindow -= Time.deltaTime;
		else 
			tapCount = 0;
		
		//no fingers are touching, so we reset the position
		
		if (count == 0)
			Reset();	
		else 
		{
			for (int i = 0 ; i < count; i++) 
			{
				Touch touch = Input.GetTouch(i);
				Vector2 guiTouchPos = touch.position - guiTouchOffset;
				
				// Latch the finger if this is a new touch
				if ( gui.HitTest(touch.position ) && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) ) 
				{
					lastFingerId = touch.fingerId;
					
					// Accumulate taps if it is within the time window
					if ( tapTimeWindow > 0 )
						tapCount++;
					else 
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}
					
					// Tell other joysticks we've latched this finger
					foreach (Joystick j in joysticks) 
					{
						if ( j != this )
							j.LatchedFinger(touch.fingerId);
					}
				}
					
				if (lastFingerId == touch.fingerId)
				{
					// Override the tap count with what the iPhone SDK reports if it is greater
					// This is a workaround, since the iPhone SDK does not currently track taps
					// for multiple touches
					if ( touch.tapCount > tapCount )
						tapCount = touch.tapCount;
					
					// Change the location of the joystick graphic to match where the touch is
					Rect tempRect = gui.pixelInset;
					tempRect.x = Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
					tempRect.y = Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );
					gui.pixelInset = tempRect;
					
					//Another check to see if the fingers are touching
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
						Reset();
				}
			}
		}
		
		// Get a value between -1 and 1
		position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
		position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
		
		// Adjust for dead zone
		float absoluteX = Mathf.Abs(position.x);
		float absoluteY = Mathf.Abs(position.y);
		
		if ( absoluteX < deadZone.x ) 
		{
			// Report the joystick as being at the center if it is within the dead zone
			position.x = 0;
			
		} 
		else if (normalize)
		{
			// Rescale the output after taking the dead zone into account
			position.x = Mathf.Sign(position.x) * (absoluteX - deadZone.x) / (1 - deadZone.x);
		}
				
		if ( absoluteY < deadZone.y ) 
		{
			// Report the joystick as being at the center if it is within the dead zone
			position.y = 0;
			
		} 
		else if (normalize)
		{
			// Rescale the output after taking the dead zone into account
			position.y = Mathf.Sign(position.y) * (absoluteY - deadZone.y) / (1 - deadZone.y);
		}
	}
}

