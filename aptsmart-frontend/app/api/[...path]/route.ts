import { NextRequest, NextResponse } from "next/server";

const backendURL = process.env.BACKEND_URL;
if (!backendURL) {
  throw new Error("BACKEND_URL is not set. Please configure it in your environment variables.");
}

export async function handler(req: NextRequest, { params }: { params: { path: string[] } }) {
    // Convert the path array to a valid backend API path
    const backendPath = (await params).path.join("/");
    const url = `${backendURL}/api/${backendPath}`;

    console.log(url);

    try {
        const backendResponse = await fetch(url, {
        method: req.method,
        headers: {
            "Content-Type": req.headers.get("Content-Type") || "application/json",
            Cookie: req.headers.get("cookie") || "", // Forward cookies for authentication
            Authorization: req.headers.get("Authorization") || "", // Forward auth headers
        },
        body: req.method !== "GET" && req.method !== "DELETE" ? await req.text() : undefined,
        });

        // Detect response type (JSON or text)
        const contentType = backendResponse.headers.get("Content-Type");
        let responseData;
        if (contentType?.includes("application/json")) {
            responseData = await backendResponse.json();
            return NextResponse.json(responseData, { 
                status: backendResponse.status,
                headers: backendResponse.headers
            })
        } else {
            responseData = await backendResponse.text();
            return new NextResponse(responseData, {
                status: backendResponse.status,
                headers: backendResponse.headers
            })
        }

    } catch (error) {
        return NextResponse.json({ error: "Failed to fetch from backend" }, { status: 500 });
    }
}

export { handler as GET, handler as POST, handler as PUT, handler as DELETE };
