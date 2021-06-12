# Restaurant-Picker
We are given a CSV file containing the prices of every item on every menu of every restaurant in the town. In addition, the restaurants also offer Value Meals, which are groups of several items, at a discounted price. For example:

Data File data.csv

1, 4.00, burger 1, 8.00, tofu_log
2, 5.00, burger 2, 6.50, tofu_log

Program Input
> program data.csv burger tofu_log

Expected Output
=> 2, 11.5

Data File data.csv

3, 4.00, chef_salad
3, 8.00, steak_salad_sandwich
4, 5.00, steak_salad_sandwich
4, 2.50, wine_spritzer

Program Input
> program data.csv chef_salad, wine_spritzer

Expected Output
=> null

Data File data.csv

5, 4.00, extreme_fajita
5, 8.00, fancy_european_water
6, 5.00, fancy_european_water
6, 6.00, extreme_fajita, jalapeno_poppers, extra_salsa

Program Input
 > program data.csv fancy_european_water extreme_fajita

Expected Output
=> 6, 11.0

## This project outputs the restaurant they should go to and the total price it will cost them that is the minimum or the most economical one.
