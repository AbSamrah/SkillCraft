import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';
import { API_BASE_URL } from './config.js';

// Create a custom trend metric. We can reuse this in other tests too.
export const getAllRoadmapsDuration = new Trend('get_all_roadmaps_duration');

// This is the main test logic, exported as a function.
export function getAllRoadmapsTest() {
    const url = `${API_BASE_URL}/Roadmaps`;

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const res = http.get(url, params);

    // Add the request duration to our custom metric
    getAllRoadmapsDuration.add(res.timings.duration);

    // Check the response
    check(res, {
        'get all successful (status 200)': (r) => r.status === 200,
        'response body contains roadmaps': (r) => r.body.length > 100,
    });

    sleep(1);
}

