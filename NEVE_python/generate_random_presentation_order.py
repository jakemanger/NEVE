import random
import sys


def main(min_num, max_num, num_times, seed=12345):
    random.seed(seed)

    for _ in range(num_times):
        randomlist = list(range(min_num, max_num))
        random.shuffle(randomlist) 

        print(randomlist)


if __name__ == "__main__":
    if len(sys.argv) != 4:
        print(
            '''
            Usage: generate_random_presentation_order.py <min_num> <max_num> <num_times>
            '''
        )
        sys.exit(1)

    main(int(sys.argv[1]), int(sys.argv[2]), int(sys.argv[3]))



