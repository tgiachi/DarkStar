using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Utils;
using DarkStar.Engine.Attributes.ScriptEngine;
using GoRogue.DiceNotation;
using GoRogue.DiceNotation.Terms;

namespace DarkStar.Engine.ScriptModules;

[ScriptModule]
public class RandomUtilsScriptModule
{
    [ScriptFunction("random_range")]
    public int Random(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max);
    }

    [ScriptFunction("random_bool")]
    public bool RandomBool() => RandomUtils.RandomBool();

    [ScriptFunction("parse_dice_exp")]
    public int ParseDice(string expression) => Dice.Parse(expression).Roll();
}
