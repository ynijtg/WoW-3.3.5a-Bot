﻿using AmeisenBotCombat.Objects;
using AmeisenBotManager;
using AmeisenBotUtilities;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AmeisenBotGUI
{
    /// <summary>
    /// Interaktionslogik für CombatClassEditor.xaml
    /// </summary>
    public partial class CombatClassWindow : Window
    {
        private CombatLogic loadedLogic;
        private int prio;
        private BotManager BotManager { get; }

        public CombatClassWindow(BotManager botManager)
        {
            InitializeComponent();
            BotManager = botManager;

            string defaultCombatClass = BotManager.Settings.combatClassPath;
            if (defaultCombatClass != "none")
                loadedLogic = AmeisenBotCombat.AmeisenCombatEngine.LoadCombatLogicFromFile(defaultCombatClass);
            else
                loadedLogic = new CombatLogic();

            Topmost = BotManager.Settings.topMost;
        }

        private void ButtonAddCombatEntry_Click(object sender, RoutedEventArgs e)
        {
            listboxCombatActions.Items.Add(new CombatLogicEntry());
            listboxCombatActions.SelectedIndex = 0;

            textboxPriority.Text = prio.ToString();
            prio++;

            buttonAddCondition.IsEnabled = true;
            buttonRemoveCondition.IsEnabled = true;
        }

        private void ButtonAddCondition_Click(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
            {
                AmeisenBotCombat.Objects.Condition condition = new AmeisenBotCombat.Objects.Condition();

                ((CombatLogicEntry)listboxCombatActions.SelectedItem).Conditions.Add(condition);
                listboxConditions.Items.Add(condition);

                listboxConditions.SelectedIndex = 0;

                comboboxLuaUnitOne.IsEnabled = true;
                comboboxLuaUnitTwo.IsEnabled = true;
                comboboxValueOne.IsEnabled = true;
                comboboxValueTwo.IsEnabled = true;
                comboboxValueOperator.IsEnabled = true;
                textboxCustomValue.IsEnabled = true;
                radiobuttonCustomValue.IsEnabled = true;
                radiobuttonPreValue.IsEnabled = true;
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            loadedLogic = new CombatLogic();
            LoadEntries();
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                AddExtension = true,
                RestoreDirectory = true,
                Filter = "CombatClass JSON|*.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                loadedLogic.combatLogicEntries.Clear();
                loadedLogic = AmeisenBotCombat.AmeisenCombatEngine.LoadCombatLogicFromFile(openFileDialog.FileName);

                if (loadedLogic.combatLogicEntries != null)
                {
                    listboxCombatActions.Items.Clear();
                    listboxConditions.Items.Clear();
                    foreach (CombatLogicEntry action in loadedLogic.combatLogicEntries)
                        listboxCombatActions.Items.Add(action);
                    listboxCombatActions.SelectedIndex = 0;
                }
            }
        }

        private void ButtonRemoveCombatEntry_Click(object sender, RoutedEventArgs e)
        {
            listboxCombatActions.Items.RemoveAt(listboxCombatActions.SelectedIndex);
        }

        private void ButtonRemoveCondition_Click(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
            {
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).Conditions.RemoveAt(listboxCombatActions.SelectedIndex);
                listboxConditions.Items.RemoveAt(listboxCombatActions.SelectedIndex);
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            loadedLogic.combatLogicEntries.Clear();
            foreach (CombatLogicEntry action in listboxCombatActions.Items)
                loadedLogic.combatLogicEntries.Add(action);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                RestoreDirectory = true,
                Filter = "CombatClass JSON|*.json"
            };

            string defaultCombatClass = BotManager.Settings.combatClassPath;
            if (defaultCombatClass == "none")
                saveFileDialog.FileName = "sampleCombatClass.json";
            else if (Path.GetFileNameWithoutExtension(defaultCombatClass) != "json")
                saveFileDialog.FileName = defaultCombatClass + ".json";
            else
                saveFileDialog.FileName = defaultCombatClass;

            if (saveFileDialog.ShowDialog() == true)
                AmeisenBotCombat.AmeisenCombatEngine.SaveToFile(saveFileDialog.FileName, loadedLogic);
        }

        private void CheckboxCanCastDuringMovement_Checked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).CanMoveDuringCast = true;
        }

        private void CheckboxCanCastDuringMovement_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).CanMoveDuringCast = false;
        }

        private void CheckboxCombatOnly_Checked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).CombatOnly = true;
        }

        private void CheckboxCombatOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).CombatOnly = false;
        }

        private void CheckboxIsBuff_Checked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).IsBuff = true;
            checkboxIsForParty.IsEnabled = true;
        }

        private void CheckboxIsBuff_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).IsBuff = false;
            checkboxIsForParty.IsEnabled = false;
        }

        private void CheckboxIsForMySelf_Checked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).IsForMyself = true;
        }

        private void CheckboxIsForMySelf_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).IsForMyself = false;
        }

        private void CheckboxIsForParty_Checked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).IsBuffForParty = true;
        }

        private void CheckboxIsForParty_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).IsBuffForParty = false;
        }

        private void ComboboxAction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).Action = (CombatLogicAction)comboboxAction.SelectedItem;
        }

        private void ComboboxActionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).ActionType = (CombatActionType)comboboxActionType.SelectedItem;
        }

        private void ComboboxLuaUnitOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxConditions.SelectedItem != null)
                ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionLuaUnits[0] = (LuaUnit)comboboxLuaUnitOne.SelectedItem;
        }

        private void ComboboxLuaUnitTwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxConditions.SelectedItem != null)
                ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionLuaUnits[1] = (LuaUnit)comboboxLuaUnitTwo.SelectedItem;
        }

        private void ComboboxValueOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxConditions.SelectedItem != null)
                ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionValues[0] = (CombatLogicValues)comboboxValueOne.SelectedItem;
        }

        private void ComboboxValueOperator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxConditions.SelectedItem != null)
            {
                CombatLogicStatement op = (CombatLogicStatement)comboboxValueOperator.SelectedItem;
                AmeisenBotCombat.Objects.Condition cond = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem);

                cond.statement = op;

                listboxConditions.SelectedItem = cond;
            }
        }

        private void ComboboxValueTwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listboxConditions.SelectedItem != null)
                ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionValues[1] = (CombatLogicValues)comboboxValueTwo.SelectedItem;
        }

        private void GuiCombatClassEditor_Loaded(object sender, RoutedEventArgs e)
        {
            comboboxAction.Items.Add(CombatLogicAction.USE_SPELL);
            comboboxAction.Items.Add(CombatLogicAction.USE_AOE_SPELL);
            comboboxAction.Items.Add(CombatLogicAction.SHAPESHIFT);
            comboboxAction.Items.Add(CombatLogicAction.FLEE);

            comboboxValueOne.Items.Add(CombatLogicValues.HP);
            comboboxValueOne.Items.Add(CombatLogicValues.ENERGY);
            //comboboxValueOne.Items.Add(CombatLogicValues.TARGET_IS_CASTING);

            comboboxValueTwo.Items.Add(CombatLogicValues.HP);
            comboboxValueTwo.Items.Add(CombatLogicValues.ENERGY);

            comboboxValueOperator.Items.Add(CombatLogicStatement.GREATER);
            comboboxValueOperator.Items.Add(CombatLogicStatement.GREATER_OR_EQUAL);
            comboboxValueOperator.Items.Add(CombatLogicStatement.EQUAL);
            comboboxValueOperator.Items.Add(CombatLogicStatement.LESS_OR_EQUAL);
            comboboxValueOperator.Items.Add(CombatLogicStatement.LESS);
            comboboxValueOperator.Items.Add(CombatLogicStatement.HAS_BUFF);
            comboboxValueOperator.Items.Add(CombatLogicStatement.NOT_HAS_BUFF);

            comboboxActionType.Items.Add(CombatActionType.ATTACK);
            comboboxActionType.Items.Add(CombatActionType.HEAL);
            comboboxActionType.Items.Add(CombatActionType.TANK);
            comboboxActionType.Items.Add(CombatActionType.BUFF);

            comboboxLuaUnitOne.Items.Add(LuaUnit.player);
            comboboxLuaUnitOne.Items.Add(LuaUnit.target);
            comboboxLuaUnitOne.Items.Add(LuaUnit.party1);
            comboboxLuaUnitOne.Items.Add(LuaUnit.party2);
            comboboxLuaUnitOne.Items.Add(LuaUnit.party3);
            comboboxLuaUnitOne.Items.Add(LuaUnit.party4);

            comboboxLuaUnitTwo.Items.Add(LuaUnit.player);
            comboboxLuaUnitTwo.Items.Add(LuaUnit.target);
            comboboxLuaUnitTwo.Items.Add(LuaUnit.party1);
            comboboxLuaUnitTwo.Items.Add(LuaUnit.party2);
            comboboxLuaUnitTwo.Items.Add(LuaUnit.party3);
            comboboxLuaUnitTwo.Items.Add(LuaUnit.party4);

            LoadEntries();
        }

        private void GuiCombatClassEditor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void ListboxCombatActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CombatLogicEntry entry = ((CombatLogicEntry)listboxCombatActions.SelectedItem);
                textboxPriority.Text = entry.Priority.ToString();
                textboxMaxDistance.Text = entry.MaxSpellDistance.ToString();
                textboxSpellName.Text = (string)entry.Parameters;

                checkboxCanCastDuringMovement.IsChecked = entry.CanMoveDuringCast;
                checkboxCombatOnly.IsChecked = entry.CombatOnly;
                checkboxIsBuff.IsChecked = entry.IsBuff;
                checkboxIsForParty.IsChecked = entry.IsBuffForParty;
                checkboxIsForMySelf.IsChecked = entry.IsForMyself;

                listboxConditions.Items.Clear();
                foreach (AmeisenBotCombat.Objects.Condition c in entry.Conditions)
                    listboxConditions.Items.Add(c);

                comboboxActionType.SelectedItem = entry.ActionType;

                if (listboxConditions.Items.Count > 0)
                {
                    comboboxLuaUnitOne.IsEnabled = true;
                    comboboxLuaUnitTwo.IsEnabled = true;
                    comboboxValueOne.IsEnabled = true;
                    comboboxValueTwo.IsEnabled = true;
                    comboboxValueOperator.IsEnabled = true;
                    textboxCustomValue.IsEnabled = true;
                    radiobuttonCustomValue.IsEnabled = true;
                    radiobuttonPreValue.IsEnabled = true;
                }
            }
            catch { }
        }

        private void ListboxConditions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
            {
                if ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem != null)
                {
                    radiobuttonCustomValue.IsChecked = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).customSecondValue;

                    if (radiobuttonCustomValue.IsChecked == true)
                        textboxCustomValue.Text = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).customValue.ToString();
                    else
                        comboboxValueTwo.SelectedItem = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionValues[1];

                    comboboxValueOne.SelectedItem = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionValues[0];
                    comboboxValueOperator.SelectedItem = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).statement;

                    comboboxLuaUnitOne.SelectedItem = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionLuaUnits[0];
                    comboboxLuaUnitTwo.SelectedItem = ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).conditionLuaUnits[1];
                }
            }
        }

        private void LoadEntries()
        {
            listboxCombatActions.Items.Clear();
            foreach (CombatLogicEntry entry in loadedLogic.combatLogicEntries)
                listboxCombatActions.Items.Add(entry);
            prio = listboxCombatActions.Items.Count;

            if (listboxCombatActions.Items.Count > 0)
            {
                buttonAddCondition.IsEnabled = true;
                buttonRemoveCondition.IsEnabled = true;
            }
        }

        private void RadiobuttonCustomValue_Checked(object sender, RoutedEventArgs e)
        {
            if (textboxCustomValue != null && (AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem != null)
            {
                comboboxValueTwo.IsEnabled = false;
                comboboxLuaUnitTwo.IsEnabled = false;
                textboxCustomValue.IsEnabled = true;
                ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).customSecondValue = true;
            }
        }

        private void RadiobuttonPreValue_Checked(object sender, RoutedEventArgs e)
        {
            if (textboxCustomValue != null && (AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem != null)
            {
                comboboxValueTwo.IsEnabled = true;
                comboboxLuaUnitTwo.IsEnabled = true;
                textboxCustomValue.IsEnabled = false;
                ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).customSecondValue = false;
            }
        }

        private void TextboxCustomValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listboxConditions.SelectedItem != null && textboxCustomValue.Text.Length > 0)
                ((AmeisenBotCombat.Objects.Condition)listboxConditions.SelectedItem).customValue = textboxCustomValue.Text;
        }

        private void TextboxMaxDistance_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).MaxSpellDistance = float.Parse(textboxMaxDistance.Text);
        }

        private void TextboxPriority_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).Priority = int.Parse(textboxPriority.Text);
        }

        private void TextboxSpellName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((CombatLogicEntry)listboxCombatActions.SelectedItem) != null)
                ((CombatLogicEntry)listboxCombatActions.SelectedItem).Parameters = textboxSpellName.Text;
        }
    }
}