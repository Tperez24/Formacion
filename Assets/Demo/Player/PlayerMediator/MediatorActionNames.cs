using System;
using System.Collections.Generic;

namespace Demo.Player.PlayerMediator
{
    public static class MediatorActionNames
    {
        //Subscriptions to player movement controller
        public static string PlayerMoveSubscription() => "PlayerMoveSubscription";
        
        //SpellController and AttackController
        public static string LaunchAttack() => "LaunchAttack";
        public static string LaunchSpecialAttack() => "LaunchSpecialAttack";
        public static string AimSpecialAttack() => "AimSpecialAttack";
        public static string SpecialAttackCanceled() => "SpecialAttackCanceled";
        
        //Trigger Player Animations
        public static string TriggerSpecialAttack() => "TriggerSpecialAttack";
        public static string PausePlayerAnimator() => "TriggerSpecialAttack";
        public static string ResumePlayerAnimator() => "TriggerSpecialAttack";
        public static string TriggerNormalAttack() => "TriggerNormalAttack";
        
        //GetReferences
        public static string AttackController() => "AttackController";
        public static string SpellController() => "SpellController";
        public static string PlayerPosition() => "PlayerPosition";
        public static string CompositeAnimator() => "CompositeAnimator";
        public static string CompositeType() => "CompositeType";
        public static string GetPlayerAnimator() => "GetPlayerAnimator";
    }
}