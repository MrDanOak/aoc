struct Lists {
    first: Vec<i32>,
    second: Vec<i32>,
}

fn lists() -> Lists {
    let file  = include_str!("../inputs/day1.txt");
    let mut lines = file
        .split("\n")
        .collect::<Vec<&str>>();

    let mut lists = Lists { first: Vec::new(), second: Vec::new(), };

    for l in lines.iter_mut() {
        let parts = l.split(" ").filter(|&x| !x.is_empty()).collect::<Vec<&str>>();
        lists.first.push(parts[0].parse::<i32>().unwrap());
        lists.second.push(parts[1].parse::<i32>().unwrap());
    }

    lists.first.sort();
    lists.second.sort();

    lists
}

pub fn part1() -> String {
    let lists = lists();
    let mut acc = 0;
    for i in 0..lists.first.len() {
        acc += (lists.first[i] - lists.second[i]).abs()
    }
    acc.to_string()
}

pub fn part2() -> String {
    let lists = lists();
    let mut acc = 0;
    for i in 0..lists.first.len() {
        acc += lists.first[i] * lists.second.iter().filter(|&&x| x == lists.first[i]).count() as i32
    }
    acc.to_string()
}
