//Luke F 08/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{

    //we can parent this to the player or have it as a seperate game object. Need to consider that the controls can be changed before a player
    //exists
    public PlayerController playerController;


    public Command _AButton = new Command();
    public Command _BButton = new Command();

    public Command _LeftBumperDown = new Command();
    public Command _LeftBumperUp = new Command();

    //etc...

    //This will be the fuction that Ben or whomever can use to change the buttons in the main menu
    public Command SetButton(Command action)
    {
        return action; 
    }

    private void InitializeDefaultControls()
    {
        //mapping the defaultControls
        JumpCommand jump = new JumpCommand();
        _AButton = SetButton(jump);

        InitCrouchCommand crouch = new InitCrouchCommand();
        _BButton = SetButton(crouch);

        InitiateGrappleCommand grap = new InitiateGrappleCommand();
        _LeftBumperDown = SetButton(grap);

        DeatchGrappleCommand dGrap = new DeatchGrappleCommand();
        _LeftBumperUp = SetButton(dGrap);
    }

    private void FixedUpdate()
    {
        //if this seems redundant, that only because we've named the XBOX A_Button "jump" in the Unity Editor  
        if (Input.GetButtonDown("Jump"))
        {
            _AButton.Execute(playerController);
        }
        if (Input.GetButtonDown("RightBumper"))
        {
            _BButton.Execute(playerController);
        }
        if (Input.GetButtonUp("RightBumper"))
        {

        }
        if (Input.GetButtonDown("LeftBumper"))//p for testing
        {
            _LeftBumperDown.Execute(playerController);
        }


        if (Input.GetButtonUp("LeftBumper"))
        {
            _LeftBumperUp.Execute(playerController);

        }
    }




    private void Start()
    {
        InitializeDefaultControls();
    }
}





public class Command
{
    
    public virtual void Execute(PlayerController playCont) { }

}

public class JumpCommand : Command
{
    public override void Execute(PlayerController playCont)
    {
        playCont.Jump();
      
    }
}


public class InitiateGrappleCommand : Command
{
    public override void Execute(PlayerController playCont)
    {
       playCont.Grapple();
    }
}

public class DeatchGrappleCommand : Command
{
    public override void Execute(PlayerController playCont)
    {
        playCont.DetatchGrapple();
    }
}

public class InitCrouchCommand : Command
{
    public override void Execute(PlayerController playCont)
    {
        playCont.Crouch();
        
    }
}

public class DeaCrouchCommand : Command
{
    public void Execute(PlayerClass playClass)
    {
        playClass.SetMovementType("move");
    }
}