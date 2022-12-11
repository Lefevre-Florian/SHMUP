using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.UI {
	
    public class Hud : Screen
    {
        [Export] private string lifeExtenstion;

        [Export] private NodePath lifePath;
        [Export] private NodePath scorePath;
        [Export] private NodePath smartBombPath;

        private Label score;
        private HBoxContainer smartBomb;

        private List<Polygon2D> hearts = new List<Polygon2D>();

        private uint nScore = 0;
        private int heartsLength = 0;

        public override void _Ready()
        {
            foreach (Polygon2D lHeart in GetNode<HBoxContainer>(lifePath).GetChildren())
                hearts.Add(lHeart);

            score = GetNode<Label>(scorePath);
            smartBomb = GetNode<HBoxContainer>(smartBombPath);

            heartsLength = hearts.Count-1;

        }

        public void UpdateLifeHUD(int pDamage, bool pDelete = true)
        {
            int lLength = heartsLength - pDamage;
            for (int i = heartsLength; i >= lLength; i--)
                hearts[i].Visible = pDelete;
        }

        public void UpdateSmartBombHUD()
        {

        }

        public void UpdateScoreHUD(uint pScore)
        {
            nScore += pScore;
            score.Text = $"{lifeExtenstion}:\n{nScore}";
        }

    }
}