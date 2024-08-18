Implemented the coding test for fishing game using C# and .NET 8.0 following the requirements:

1. Player starts from 100 gold
2. Player could rent small, medium or big fishing pole with a cost of 5, 10 and 15 respectively with the ability to only
   catch small, medium or big fish depending on which pole the player rented.
3. Player can buy red, blue or green baits. Each guaranteed to catch respective colored fish with a cost of 1, 2 and 3
   gold respectively. Player can only continue to fish only if they have spent all their money!
4. The fishes that gets caught have their own sale price, red and blue fishes are randomized ranged value while green
   fishes are a constant value.
5. Each day a forecast telling player shall be shown to give player a chance to plan out their day of fishing consisting
   of fish amounts of each types and how much percentage of each fish color. The percentage itself is ensured to always
   be 100%.
6. When done fishing, the performance shall be judged. Player will win if they have > 100 gold, lose if they have <= 100
   and tie if they decided to skip the day.

I also make sure to have an easy to configure class - GameConstants.cs - so that tweaking values can be a bit easier,
whether for designer to play around or just testing things out.

Things added from the original design:

1. Auto rent and buy baits based on the current forecast using weights calculated.
2. Wait for space bar input to simulate fishing.
3. Pick which bait to use before casting bait.
4. Choice to skip the day before actually start fishing and while fishing (if they choose to end the day early).
5. Delay between actions for fishing and judging performance to add some "tension" for player.

Things I would like to add if this ever gets expanded:

1. I would like to have a persistent inventory of baits that won't get reset each day within current session or just
   have it as a 'save game'
2. I would also involve weather (raining, sunny) could affect the forecast result
3. I think having some dynamic change of situation at the pond after each fish attempt could add some dynamic to the
   game, for example during prolonged use of fishing pole, the rod could be more loose thus resulting in player can only
   caught smaller type of fish that the pole could actually pull, or change of weather make some fish no longer swim
   around fishing area and have a new type of fish that only appear when this change of weather occur.