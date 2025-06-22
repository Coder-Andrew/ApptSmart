import { NextResponse } from "next/server";
import { NextRequest } from "next/server";
import ROUTES from "./lib/routes";

export function middleware(req: NextRequest) {
    const token = req.cookies.get("AuthToken")?.value;
    const res = NextResponse.next();

    const protectedRoutes = ["/c/", "/appointments"];
    const protectedEndpoints = ["/owner"];

    if (!token && (
        protectedRoutes.some(r => req.nextUrl.pathname.startsWith(r)) || 
        protectedEndpoints.some(r => req.nextUrl.pathname.endsWith(r))
    )) {
        res.headers.set('x-require-auth', 'true');
        const redirectUrl = new URL(ROUTES.home, req.url);
        redirectUrl.searchParams.set('redirect', req.nextUrl.pathname);
        return NextResponse.redirect(redirectUrl);
    }

    return NextResponse.next();
}

export const config = {
    matcher: [
        '/c/:path*',
        '/appointments',
        "/:path*/owner"
    ]
}