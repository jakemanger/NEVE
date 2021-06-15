import random

rand_num_min = 0
# rand_num_max = 12 
# rand_num_max = 15 
rand_num_max = 47 

# random.seed(936)
# random.seed(342)
random.seed(451)

for i in range(0, 30):
    randomlist = list(range(rand_num_min, rand_num_max))
    random.shuffle(randomlist) 

    print(randomlist)