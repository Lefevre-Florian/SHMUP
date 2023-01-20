using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.Environment {

	public class Box : Node2D
	{

		[Export] private NodePath animationPath = null;	
		[Export(PropertyHint.File)] public Animation animation = null;

		private AnimationPlayer animator;

		public override void _Ready()
		{
			animator = GetNode<AnimationPlayer>(animationPath);
			animator.AssignedAnimation = animation.ResourceName;

			animation.Loop = false;
			animator.Play();

			GetTree().CreateTimer(animation.Length).Connect(EventTimer.TIMEOUT, this, nameof(SetAnimationEnd));
		}

		private void SetAnimationEnd()
        {
			QueueFree();
        }

	}

}