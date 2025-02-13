"use client";

import Link from "next/link";
import { useState } from "react";

export default function SideBar() {
  const [isMenuOpen, setIsMenuOpen] = useState(true);

  return (
    <div
      className={`bg-gray-800 text-white transition-all duration-300 ${isMenuOpen ? "w-64" : "w-16"}`}
    >
      <button
        onClick={() => setIsMenuOpen(!isMenuOpen)}
        className="p-4 w-full flex justify-center hover:bg-gray-700"
      >
        {isMenuOpen ? "◄" : "►"}
      </button>
      <nav className="p-4">
        <ul className="space-y-4">
          <li>
            <Link
              href="/agents"
              className="flex items-center hover:text-gray-300"
            >
              <span className="material-icons mr-2">person</span>
              {isMenuOpen && <span>Agents</span>}
            </Link>
          </li>
          <li>
            <Link
              href="/rooms"
              className="flex items-center hover:text-gray-300"
            >
              <span className="material-icons mr-2">meeting_room</span>
              {isMenuOpen && <span>Rooms</span>}
            </Link>
          </li>
          <li>
            <Link
              href="/conversations"
              className="flex items-center hover:text-gray-300"
            >
              <span className="material-icons mr-2">chat</span>
              {isMenuOpen && <span>Conversations</span>}
            </Link>
          </li>
        </ul>
      </nav>
    </div>
  );
}
