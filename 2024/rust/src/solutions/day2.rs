use std::cmp;

fn safe_distance(a: i32, b: i32) -> bool {
    (1..=3).contains(&(cmp::max(a, b) - cmp::min(a, b)))
}

fn safe_report(report: Vec<i32>) -> bool {
    let increasing = report[0] < report[1];
    for i in 1..report.len() {
        let next_increasing = report[i - 1] < report[i];
        let safe_distance = safe_distance(report[i], report[i - 1]);
        if safe_distance && ((increasing && next_increasing) || (!increasing && !next_increasing)) {
            continue;
        }
        return false;
    }
    true
}

fn reports() -> Vec<Vec<i32>> {
    include_str!("../inputs/day1.txt")
        .split("\n")
        .collect::<Vec<&str>>()
        .iter()
        .map(|&s| {
            s.split(" ")
                .map(|s| s.parse::<i32>().expect("Should be able to parse numbers"))
                .collect::<Vec<i32>>()
        })
        .collect::<Vec<Vec<i32>>>()
}

pub fn part1() -> String {
    let reports = reports();

    let safeReports = reports.iter().filter(|&&report| {
        let increasing = match report.get(0) {
            Some(a) => match report.get(1) {
                Some(b) => a > b,
                _ => false,
            },
            _ => false,
        };

        return false;
    });
}

pub fn part2() -> String {}
