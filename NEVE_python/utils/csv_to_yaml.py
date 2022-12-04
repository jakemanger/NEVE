# use argparse to read a csv file and convert to list
# output

import argparse
import csv


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("csvfile", help="csv file to convert")
    args = parser.parse_args()

    # read csv file and save each column value to list
    # then print
    with open(args.csvfile, 'r') as f:
        reader = csv.reader(f)
        for column in zip(*reader):
            print(column[0])
            # remove column name
            column = column[1:]
            # if they are numbers, convert to float
            try:
                column = [float(x) for x in column]
            except ValueError:
                pass
            # convert to ints if they round to ints
            if all([x.is_integer() for x in column]):
                column = [int(x) for x in column]

            print(list(column))


if __name__ == "__main__":
    main()
