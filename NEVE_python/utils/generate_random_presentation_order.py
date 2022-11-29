import random
import sys


def main(min_num, max_num, num_times, num_repeats, seed=12345):
    random.seed(seed)

    for _ in range(num_times):
        randomlist = list(range(min_num, max_num))
        randomlist = randomlist * num_repeats
        random.shuffle(randomlist) 

        print(randomlist)


if __name__ == "__main__":
    if len(sys.argv) != 5:
        print(
            '''
            Usage: generate_random_presentation_order.py <min_num> <max_num> <num_times> <num_repeats>
            '''
        )
        sys.exit(1)

    main(int(sys.argv[1]), int(sys.argv[2]), int(sys.argv[3]), int(sys.argv[4]))



