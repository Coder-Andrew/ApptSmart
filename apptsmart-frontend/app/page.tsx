import Link from "next/link";

export default function Home() {

  return (
    <>
      <h1>Welcome to AptSmart!</h1>
      <p>The website where you can handle all of your scheduling and appointment needs!</p>
      <Link href={"/test"}>Test</Link>
      <Link href={"/schedule"}>Schedule an appointment!</Link>
    </>
  );
}
