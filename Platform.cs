﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace FinalTestingGround
{
    internal class Platform
    {
        Texture2D platText;
        Rectangle platRec;
        Color platColor;
        int boundTop, boundBottom, boundLeft, boundRight;
        int speed, maxammo, ammo, reload, charge, maxcharge, cooldown, maxcooldown;
        List<projectile> projectiles;
        

        // UI elements
        Texture2D chargeSprite;
        Texture2D ammoSprite;
        Vector2 chargePosition;
        Vector2 ammoPosition;

        public Platform(Texture2D platText, Rectangle platRec, Color platColor, int boundTop,
            int boundBottom, int boundLeft, int boundRight, int speed, int ammo, int maxammo,
            int reload, int charge, int maxcharge, int cooldown, int maxcooldown, List<projectile> projectiles,
            Texture2D chargeSprite, Texture2D ammoSprite, Vector2 chargePosition, Vector2 ammoPosition)
        {
            this.platText = platText;
            this.platRec = platRec;
            this.platColor = platColor;
            this.boundTop = boundTop;
            this.boundBottom = boundBottom;
            this.boundLeft = boundLeft;
            this.boundRight = boundRight;
            this.speed = speed;
            this.projectiles = projectiles;
            this.ammo = ammo;
            this.maxammo = maxammo;
            this.reload = reload;
            this.charge = charge;
            this.maxcharge = maxcharge;
            this.cooldown = cooldown;
            this.maxcooldown = maxcooldown;

            this.chargeSprite = chargeSprite;
            this.ammoSprite = ammoSprite;
            this.chargePosition = chargePosition;
            this.ammoPosition = ammoPosition;



        }

        public Platform(Texture2D texture2D1, Rectangle rectangle, Color red, int v1, int wCBH, int v2, int wCBW, int speed, int v3, int v4, int v5, int v6, int v7, int v8, int v9, List<projectile> projectiles, Texture2D texture2D2, Texture2D texture2D3, Vector2 vector21, Vector2 vector22, int v10)
        {
            this.speed = speed;
            this.projectiles = projectiles;
        }

        public Texture2D PlatText { get => platText; set => platText = value; }
        public Rectangle PlatRec { get => platRec; set => platRec = value; }
        public Color PlatColor { get => platColor; set => platColor = value; }

        public void PlatformMovement(Keys upKey, Keys downKey, Keys leftKey, Keys rightKey, int player)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(upKey) && platRec.Y > boundTop)
            {
                platRec.Y -= speed;
            }
            if (state.IsKeyDown(downKey) && (platRec.Y + platRec.Height) < boundBottom)
            {
                platRec.Y += speed;
            }
            if (player == 2)
            {
                if (state.IsKeyDown(rightKey) && (platRec.X + platRec.Width) < boundRight)
                {
                    platRec.X += speed;
                }
                if (state.IsKeyDown(leftKey) && (platRec.X > boundLeft))
                {
                    platRec.X -= speed;
                }
            }
            if (player == 1)
            {
                if (state.IsKeyDown(leftKey) && (platRec.X > boundLeft))
                {
                    platRec.X -= speed;
                }
                if (state.IsKeyDown(rightKey) && (platRec.X + platRec.Width) < boundRight)
                {
                    platRec.X += speed;
                }
            }
        }

        public projectile ShootProjectile(Vector2 velocity, int player)
        {
            if (ammo > 0 && cooldown <= 0)
            {
                if (player == 1)
                {
                    Rectangle spawnPosition = new Rectangle(platRec.X + platRec.Width + 5, platRec.Y + platRec.Height / 2 - 12, 25, 25);
                    projectile newProjectile = new projectile(spawnPosition, velocity, new Rectangle());
                    projectiles.Add(newProjectile);
                    ammo--;
                    cooldown = maxcooldown;
                    return newProjectile;
                }
                else
                {
                    Rectangle spawnPosition = new Rectangle(platRec.X + platRec.Width - 75, platRec.Y + platRec.Height / 2 - 12, 25, 25);
                    projectile newProjectile = new projectile(spawnPosition, velocity, new Rectangle());
                    projectiles.Add(newProjectile);
                    ammo--;
                    cooldown = maxcooldown;
                    return newProjectile;
                }
            }
            else
            {
                return null;
            }
        }

        public void ShootControl(Keys trigger, int player)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(trigger))
            {
                if (player == 1)
                {
                    ShootProjectile(new Vector2(5, 0), player);
                }
                else if (player == 2)
                {
                    ShootProjectile(new Vector2(-5, 0), player);
                }
            }
        }

        public void UpdateProjectiles()
        {
            foreach (var projectile in projectiles)
            {
                projectile.Update();
            }
            if (charge < maxcharge && ammo < maxammo)
            {
                charge += reload;
                if (charge >= maxcharge && ammo < maxammo)
                {
                    charge = 0;
                    ammo++;
                }
            }
            if (cooldown > 0)
            {
                cooldown = cooldown - reload * 3;
            }
        }

        public void DrawProjectiles(SpriteBatch spriteBatch, Texture2D projectileTexture)
        {
            foreach (var projectile in projectiles)
            {
                spriteBatch.Draw(projectileTexture, projectile.Position, Color.White);
              


            }
        }

        public void DrawUI(SpriteBatch spriteBatch, int player)
        {
            // Draw charge
            if (player == 1)
            {
                for (int i = 0; i < charge; i++)
                {
                    spriteBatch.Draw(chargeSprite, new Vector2(platRec.X - 30, platRec.Y + i * 4), Color.White);
                }
                for (int i = 0; i < ammo; i++)
                {
                    spriteBatch.Draw(ammoSprite, ammoPosition + new Vector2(i * 17, 0), Color.White);
                }
            }
            else if (player == 2)
            {
                for (int i = 0; i < charge; i++)
                {
                    spriteBatch.Draw(chargeSprite, new Vector2(platRec.X + 50, platRec.Y + i * 4), Color.White);
                }
                for (int i = 0; i < ammo; i++)
                {
                    spriteBatch.Draw(ammoSprite, ammoPosition + new Vector2(i * -17, 0), Color.White);
                }
            }
            // Draw ammo


        }

        public void platformcollision(Rectangle ball)
        {
            if (platRec.Intersects(ball))
            {
                return;
            }
        }

        public PlayerStats WinnerCheck(int self, Lifebar lifebar, Lifebar opponent, Score score, Score OpponentScore, Score rounds, string winner, PlayerStats pstats)
        {
            if (score.ScoreCount == 2)
            {
                winner = "Player " + self + " Wins!";
                rounds.CountReset();
                rounds.ScoreCount += 1;
                OpponentScore.CountReset();
                score.CountReset();

                pstats = new PlayerStats()
                {
                    p1score = score.ScoreCount,
                    p2score = OpponentScore.ScoreCount,
                    p1life = lifebar.LifebarWidth,
                    p2life = opponent.LifebarWidth,
                    round = rounds.ScoreCount,
                    winner = winner
                };
                return pstats;

            }
            else
            { return null; }
        }

        public void damageCheck(int player, projectile projectile, Lifebar lifebar,
                    Lifebar opponent, Score score, Score opponentScore, Score rounds, PlayerStats pstats)
        {
            if (platRec.Intersects(projectile.DamageCheck) == true)
            {
                lifebar.LifebarWidth--;
                lifebar.LifebarNumber = lifebar.LifebarNumber -= 5;
              
                if (lifebar.LifebarWidth <= 0)
                {
                    lifebar.lifebarReset();
                    opponent.lifebarReset();
                    opponentScore.Updatescore();
                    rounds.ScoreCount++;
                }
                //WinnerCheck(player,lifebar, opponent, score, opponentScore, rounds,"", pstats);
                return; //decrements width is the original statement here
            }
        }
        public int Speed { get => speed; set => speed = value; }

    }
}