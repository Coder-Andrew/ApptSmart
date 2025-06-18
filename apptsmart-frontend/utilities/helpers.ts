import { Appointment, CompanyInformation, RawAppointment } from "@/lib/types"

export function toAppointment({id, startTime, endTime}: RawAppointment): Appointment {
    const start = new Date(startTime);
    const end = new Date(endTime);

    if (isNaN(start.getDate()) || isNaN(end.getTime())) {
        // TODO: Change later, maybe return null instead
        throw new Error("Invalid date string passed to toAppointment");
    }
    return {
        id: id,
        startTime: new Date(startTime),
        endTime: new Date(endTime)
    }
}

export function getCookie(name: string): string {
    return document.cookie
        .split('; ')
        .find(row => row.startsWith(name + '='))  // ensure exact match
        ?.split('=')
        .slice(1)
        .join('=') || '';
}

export function getUriSafeCookie(name: string): string {
    const raw = getCookie(name);
    return raw ? decodeURIComponent(raw) : "";
}

export function getCsrftoken(): string {
    return getUriSafeCookie("XSRF-TOKEN");
}

export function fetchBackend(input: string, init?: RequestInit): Promise<Response> {
    const basePath = process.env.NEXT_PUBLIC_BASE_PATH || ""; // For modifying base path later, for portfolio site
    const url = `${basePath}/api/backend${input.startsWith('/') ? input : `/${input}`}`;

    const method = init?.method?.toLowerCase();
    const csrfRequired = method && ["post", "put", "patch", "delete"].includes(method);

    const headers = {
        ...init?.headers,
        ...(csrfRequired && {
            'X-CSRF-TOKEN': getCsrftoken()
        }),
    }
    return fetch(url, {
        ...init,
        headers,
        credentials: 'include'
    });
}

export function slugify(input: string): string {
    if (!input) return "";
    let cpy = input;
    cpy = cpy.toLowerCase();
    cpy = cpy.replaceAll(/[^a-z0-9\s-]/g, "");
    cpy = cpy.replaceAll(/\s+/g, "-").trim();
    cpy = cpy.replaceAll(/-+/g, "-");
    return cpy;
}